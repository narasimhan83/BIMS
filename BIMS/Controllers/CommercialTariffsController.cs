using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class CommercialTariffsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommercialTariffsController> _logger;

        public CommercialTariffsController(ApplicationDbContext context, ILogger<CommercialTariffsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: CommercialTariffs
        public async Task<IActionResult> Index()
        {
            var tariffs = await _context.CommercialTariffs
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new CommercialTariff
                {
                    Id = t.Id,
                    InsurancePlanId = t.InsurancePlanId,
                    VehicleCategoryId = t.VehicleCategoryId,
                    VehicleTypeId = t.VehicleTypeId,
                    Percentage = t.Percentage,
                    MinimumPremium = t.MinimumPremium,
                    EffectiveFrom = t.EffectiveFrom,
                    EffectiveTo = t.EffectiveTo,
                    IsActive = t.IsActive,
                    CreatedDate = t.CreatedDate,

                    InsurancePlan = t.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = t.InsurancePlan.Id,
                            PlanName = t.InsurancePlan.PlanName,
                            PlanNameAr = t.InsurancePlan.PlanNameAr
                        },

                    VehicleCategory = t.VehicleCategory == null
                        ? null
                        : new VehicleCategory
                        {
                            Id = t.VehicleCategory.Id,
                            CategoryName = t.VehicleCategory.CategoryName,
                            CategoryNameAr = t.VehicleCategory.CategoryNameAr
                        },

                    VehicleType = t.VehicleType == null
                        ? null
                        : new VehicleType
                        {
                            Id = t.VehicleType.Id,
                            TypeName = t.VehicleType.TypeName,
                            TypeNameAr = t.VehicleType.TypeNameAr
                        }
                })
                .ToListAsync();

            return View(tariffs);
        }

        // GET: CommercialTariffs/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: CommercialTariffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,VehicleCategoryId,VehicleTypeId,Percentage,MinimumPremium,EffectiveFrom,EffectiveTo,IsActive")] CommercialTariff tariff)
        {
            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(CommercialTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                tariff.CreatedDate = DateTime.UtcNow;
                tariff.CreatedBy = User.Identity?.Name;
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Commercial tariff created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: CommercialTariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.CommercialTariffs.FindAsync(id);
            if (tariff == null) return NotFound();

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // POST: CommercialTariffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,VehicleCategoryId,VehicleTypeId,Percentage,MinimumPremium,EffectiveFrom,EffectiveTo,IsActive,CreatedDate,CreatedBy")] CommercialTariff tariff)
        {
            if (id != tariff.Id) return NotFound();

            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(CommercialTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tariff.ModifiedDate = DateTime.UtcNow;
                    tariff.ModifiedBy = User.Identity?.Name;
                    _context.Update(tariff);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Commercial tariff updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommercialTariffExists(tariff.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.VehicleCategoryId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: CommercialTariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.CommercialTariffs
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tariff == null) return NotFound();

            return View(tariff);
        }

        // POST: CommercialTariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariff = await _context.CommercialTariffs.FindAsync(id);
            if (tariff != null)
            {
                _context.CommercialTariffs.Remove(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Commercial tariff deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CommercialTariffExists(int id)
        {
            return _context.CommercialTariffs.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedVehicleCategoryId = null,
            int? selectedVehicleTypeId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Insurance Plans (project to SelectListItem to avoid legacy NULL issues)
            var planItems = await _context.InsurancePlans
                .OrderBy(p => p.PlanName)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = currentLang == "ar" && !string.IsNullOrEmpty(p.PlanNameAr)
                        ? p.PlanNameAr
                        : p.PlanName
                })
                .ToListAsync();

            if (selectedPlanId.HasValue)
            {
                foreach (var item in planItems)
                {
                    if (item.Value == selectedPlanId.Value.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            ViewBag.InsurancePlans = planItems;

            // Vehicle Categories
            var categories = await _context.VehicleCategories
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