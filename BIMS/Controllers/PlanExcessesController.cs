using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanExcessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanExcessesController> _logger;

        public PlanExcessesController(ApplicationDbContext context, ILogger<PlanExcessesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PlanExcesses
        public async Task<IActionResult> Index()
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var items = await _context.PlanExcesses
                .AsNoTracking()
                .OrderByDescending(pe => pe.CreatedDate)
                .Select(pe => new PlanExcess
                {
                    Id = pe.Id,
                    InsurancePlanId = pe.InsurancePlanId,
                    ExcessTypeId = pe.ExcessTypeId,
                    ValueBandId = pe.ValueBandId,
                    ExcessAmount = pe.ExcessAmount,
                    ExcessUnit = pe.ExcessUnit,
                    IsActive = pe.IsActive,
                    CreatedDate = pe.CreatedDate,
                    ModifiedDate = pe.ModifiedDate,

                    InsurancePlan = pe.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pe.InsurancePlan.Id,
                            PlanName = pe.InsurancePlan.PlanName,
                            PlanNameAr = pe.InsurancePlan.PlanNameAr
                        },

                    ExcessType = pe.ExcessType == null
                        ? null
                        : new ExcessType
                        {
                            Id = pe.ExcessType.Id,
                            Name = pe.ExcessType.Name,
                            Code = pe.ExcessType.Code
                        },

                    ValueBand = pe.ValueBand == null
                        ? null
                        : new ValueBand
                        {
                            Id = pe.ValueBand.Id,
                            DisplayName = pe.ValueBand.DisplayName,
                            DisplayNameAr = pe.ValueBand.DisplayNameAr,
                            ValueFrom = pe.ValueBand.ValueFrom,
                            ValueTo = pe.ValueBand.ValueTo
                        }
                })
                .ToListAsync();

            return View(items);
        }

        // GET: PlanExcesses/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: PlanExcesses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,ExcessTypeId,ValueBandId,ExcessAmount,ExcessUnit,IsActive")] PlanExcess planExcess)
        {
            if (ModelState.IsValid)
            {
                planExcess.CreatedDate = DateTime.UtcNow;
                planExcess.CreatedBy = User.Identity?.Name;
                _context.Add(planExcess);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan excess created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(planExcess.InsurancePlanId, planExcess.ExcessTypeId, planExcess.ValueBandId);
            return View(planExcess);
        }

        // GET: PlanExcesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var planExcess = await _context.PlanExcesses.FindAsync(id);
            if (planExcess == null)
                return NotFound();

            await PopulateDropdowns(planExcess.InsurancePlanId, planExcess.ExcessTypeId, planExcess.ValueBandId);
            return View(planExcess);
        }

        // POST: PlanExcesses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,ExcessTypeId,ValueBandId,ExcessAmount,ExcessUnit,IsActive,CreatedDate,CreatedBy")] PlanExcess planExcess)
        {
            if (id != planExcess.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    planExcess.ModifiedDate = DateTime.UtcNow;
                    planExcess.ModifiedBy = User.Identity?.Name;
                    _context.Update(planExcess);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Plan excess updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExcessExists(planExcess.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(planExcess.InsurancePlanId, planExcess.ExcessTypeId, planExcess.ValueBandId);
            return View(planExcess);
        }

        // GET: PlanExcesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var planExcess = await _context.PlanExcesses
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Id == id);

            if (planExcess == null)
                return NotFound();

            return View(planExcess);
        }

        // POST: PlanExcesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planExcess = await _context.PlanExcesses.FindAsync(id);
            if (planExcess != null)
            {
                _context.PlanExcesses.Remove(planExcess);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan excess deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanExcessExists(int id)
        {
            return _context.PlanExcesses.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedExcessTypeId = null,
            int? selectedValueBandId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Insurance Plans
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

            // Excess Types
            var excessTypes = await _context.ExcessTypes
                .OrderBy(e => e.Name)
                .ToListAsync();

            ViewBag.ExcessTypes = new SelectList(
                excessTypes,
                "Id",
                nameof(ExcessType.Name),
                selectedExcessTypeId);

            // Value Bands
            var bands = await _context.ValueBands
                .OrderBy(v => v.ValueFrom)
                .ToListAsync();

            ViewBag.ValueBands = new SelectList(
                bands,
                "Id",
                currentLang == "ar"
                    ? nameof(ValueBand.DisplayNameAr)
                    : nameof(ValueBand.DisplayName),
                selectedValueBandId);
        }
    }
}