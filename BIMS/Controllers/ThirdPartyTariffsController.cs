using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class ThirdPartyTariffsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThirdPartyTariffsController> _logger;

        public ThirdPartyTariffsController(ApplicationDbContext context, ILogger<ThirdPartyTariffsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ThirdPartyTariffs
        public async Task<IActionResult> Index()
        {
            var tariffs = await _context.ThirdPartyTariffs
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new ThirdPartyTariff
                {
                    Id = t.Id,
                    InsurancePlanId = t.InsurancePlanId,
                    EngineCapacityId = t.EngineCapacityId,
                    VehicleTypeId = t.VehicleTypeId,
                    PremiumAmount = t.PremiumAmount,
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

                    EngineCapacity = t.EngineCapacity == null
                        ? null
                        : new EngineCapacity
                        {
                            Id = t.EngineCapacity.Id,
                            CapacityFrom = t.EngineCapacity.CapacityFrom,
                            CapacityTo = t.EngineCapacity.CapacityTo,
                            DisplayName = t.EngineCapacity.DisplayName,
                            DisplayNameAr = t.EngineCapacity.DisplayNameAr
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

        // GET: ThirdPartyTariffs/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: ThirdPartyTariffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,EngineCapacityId,VehicleTypeId,PremiumAmount,EffectiveFrom,EffectiveTo,IsActive")] ThirdPartyTariff tariff)
        {
            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(ThirdPartyTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                tariff.CreatedDate = DateTime.UtcNow;
                tariff.CreatedBy = User.Identity?.Name;
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Third party tariff created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.EngineCapacityId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: ThirdPartyTariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.ThirdPartyTariffs.FindAsync(id);
            if (tariff == null) return NotFound();

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.EngineCapacityId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // POST: ThirdPartyTariffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,EngineCapacityId,VehicleTypeId,PremiumAmount,EffectiveFrom,EffectiveTo,IsActive,CreatedDate,CreatedBy")] ThirdPartyTariff tariff)
        {
            if (id != tariff.Id) return NotFound();

            if (tariff.EffectiveTo.HasValue && tariff.EffectiveTo.Value.Date < tariff.EffectiveFrom.Date)
            {
                ModelState.AddModelError(nameof(ThirdPartyTariff.EffectiveTo), "Effective To must be greater than or equal to Effective From.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tariff.ModifiedDate = DateTime.UtcNow;
                    tariff.ModifiedBy = User.Identity?.Name;
                    _context.Update(tariff);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Third party tariff updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThirdPartyTariffExists(tariff.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(tariff.InsurancePlanId, tariff.EngineCapacityId, tariff.VehicleTypeId);
            return View(tariff);
        }

        // GET: ThirdPartyTariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tariff = await _context.ThirdPartyTariffs
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tariff == null) return NotFound();

            return View(tariff);
        }

        // POST: ThirdPartyTariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariff = await _context.ThirdPartyTariffs.FindAsync(id);
            if (tariff != null)
            {
                _context.ThirdPartyTariffs.Remove(tariff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Third party tariff deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ThirdPartyTariffExists(int id)
        {
            return _context.ThirdPartyTariffs.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedEngineCapacityId = null,
            int? selectedVehicleTypeId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Insurance Plans (project only needed fields to avoid legacy NULL issues)
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

            // Engine Capacities
            var capacities = await _context.EngineCapacities
                .OrderBy(c => c.CapacityFrom)
                .ToListAsync();

            ViewBag.EngineCapacities = new SelectList(
                capacities,
                "Id",
                currentLang == "ar"
                    ? nameof(EngineCapacity.DisplayNameAr)
                    : nameof(EngineCapacity.DisplayName),
                selectedEngineCapacityId);

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