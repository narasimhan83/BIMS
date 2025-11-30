using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class ComprehensiveTariffsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprehensiveTariffsController> _logger;

        public ComprehensiveTariffsController(ApplicationDbContext context, ILogger<ComprehensiveTariffsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ComprehensiveTariffs
        public async Task<IActionResult> Index()
        {
            var tariffs = await _context.ComprehensiveTariffs
                .Include(t => t.InsurancePlan)
                .Include(t => t.ValueBand)
                .Include(t => t.VehicleCategory)
                .Include(t => t.VehicleType)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            return View(tariffs);
        }

        // GET: ComprehensiveTariffs/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: ComprehensiveTariffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,ValueBandId,VehicleCategoryId,VehicleTypeId,Percentage,MinimumPremium,Excess,EffectiveFrom,EffectiveTo,IsActive")] ComprehensiveTariff tariff)
        {
            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(ComprehensiveTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                tariff.CreatedDate = DateTime.UtcNow;
                tariff.CreatedBy = User.Identity?.Name;
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comprehensive tariff created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.ValueBandId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: ComprehensiveTariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.ComprehensiveTariffs.FindAsync(id);
            if (tariff == null) return NotFound();

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.ValueBandId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // POST: ComprehensiveTariffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,ValueBandId,VehicleCategoryId,VehicleTypeId,Percentage,MinimumPremium,Excess,EffectiveFrom,EffectiveTo,IsActive,CreatedDate,CreatedBy")] ComprehensiveTariff tariff)
        {
            if (id != tariff.Id) return NotFound();

            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(ComprehensiveTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tariff.ModifiedDate = DateTime.UtcNow;
                    tariff.ModifiedBy = User.Identity?.Name;
                    _context.Update(tariff);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Comprehensive tariff updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprehensiveTariffExists(tariff.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.ValueBandId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: ComprehensiveTariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.ComprehensiveTariffs
                .Include(t => t.InsurancePlan)
                .Include(t => t.ValueBand)
                .Include(t => t.VehicleCategory)
                .Include(t => t.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tariff == null) return NotFound();

            return View(tariff);
        }

        // POST: ComprehensiveTariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariff = await _context.ComprehensiveTariffs.FindAsync(id);
            if (tariff != null)
            {
                _context.ComprehensiveTariffs.Remove(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comprehensive tariff deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ComprehensiveTariffExists(int id)
        {
            return _context.ComprehensiveTariffs.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedValueBandId = null,
            int? selectedVehicleCategoryId = null,
            int? selectedVehicleTypeId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Insurance Plans
            var plans = await _context.InsurancePlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.PlanName)
                .ToListAsync();
            ViewBag.InsurancePlans = new SelectList(
                plans,
                "Id",
                currentLang == "ar"
                    ? nameof(InsurancePlan.PlanNameAr)
                    : nameof(InsurancePlan.PlanName),
                selectedPlanId);

            // Value Bands
            var bands = await _context.ValueBands
                .Where(v => v.IsActive)
                .OrderBy(v => v.ValueFrom)
                .ToListAsync();
            ViewBag.ValueBands = new SelectList(
                bands,
                "Id",
                currentLang == "ar"
                    ? nameof(ValueBand.DisplayNameAr)
                    : nameof(ValueBand.DisplayName),
                selectedValueBandId);

            // Vehicle Categories
            var categories = await _context.VehicleCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            ViewBag.VehicleCategories = new SelectList(
                categories,
                "Id",
                currentLang == "ar"
                    ? nameof(VehicleCategory.CategoryNameAr)
                    : nameof(VehicleCategory.CategoryName),
                selectedVehicleCategoryId);

            // Vehicle Types
            var types = await _context.VehicleTypes
                .Where(v => v.IsActive)
                .OrderBy(v => v.TypeName)
                .ToListAsync();
            ViewBag.VehicleTypes = new SelectList(
                types,
                "Id",
                currentLang == "ar"
                    ? nameof(VehicleType.TypeNameAr)
                    : nameof(VehicleType.TypeName),
                selectedVehicleTypeId);
        }
    }
}