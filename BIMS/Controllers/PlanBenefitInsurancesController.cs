using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanBenefitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanBenefitsController> _logger;

        public PlanBenefitsController(ApplicationDbContext context, ILogger<PlanBenefitsController> logger)
        {
            _context = context;
            _logger = logger;
        }

                // GET: PlanBenefits
                public async Task<IActionResult> Index()
                {
                    var currentLang = HttpContext.Session.GetString("Language") ?? "en";
        
                    var items = await _context.PlanBenefits
                        .AsNoTracking()
                        .OrderByDescending(pbi => pbi.CreatedDate)
                        .Select(pbi => new PlanBenefits
                        {
                    Id = pbi.Id,
                    InsurancePlanId = pbi.InsurancePlanId,
                    BenefitTypeId = pbi.BenefitTypeId,
                    IsCovered = pbi.IsCovered,
                    LimitAmount = pbi.LimitAmount,
                    ExcessAmount = pbi.ExcessAmount,
                    Remarks = pbi.Remarks,
                    IsActive = pbi.IsActive,
                    CreatedDate = pbi.CreatedDate,
                    ModifiedDate = pbi.ModifiedDate,

                    InsurancePlan = pbi.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pbi.InsurancePlan.Id,
                            PlanName = pbi.InsurancePlan.PlanName,
                            PlanNameAr = pbi.InsurancePlan.PlanNameAr
                        },

                    BenefitType = pbi.BenefitType == null
                        ? null
                        : new BenefitType
                        {
                            Id = pbi.BenefitType.Id,
                            BenefitTypeName = pbi.BenefitType.BenefitTypeName,
                            BenefitTypeNameAr = pbi.BenefitType.BenefitTypeNameAr
                        }
                })
                .ToListAsync();

            return View("~/Views/PlanBenefitInsurances/Index.cshtml", items);
        }

        // GET: PlanBenefits/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/PlanBenefitInsurances/Create.cshtml");
        }

        // POST: PlanBenefits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,BenefitTypeId,IsCovered,LimitAmount,ExcessAmount,Remarks,IsActive")] PlanBenefits planBenefit)
        {
            if (ModelState.IsValid)
            {
                planBenefit.CreatedDate = DateTime.UtcNow;
                planBenefit.CreatedBy = User.Identity?.Name;

                _context.Add(planBenefit);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan benefit insurance created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(planBenefit.InsurancePlanId, planBenefit.BenefitTypeId);
            return View("~/Views/PlanBenefitInsurances/Create.cshtml", planBenefit);
        }

        // GET: PlanBenefits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var planBenefit = await _context.PlanBenefits.FindAsync(id);
            if (planBenefit == null)
                return NotFound();

            await PopulateDropdowns(planBenefit.InsurancePlanId, planBenefit.BenefitTypeId);
            return View("~/Views/PlanBenefitInsurances/Edit.cshtml", planBenefit);
        }

        // POST: PlanBenefits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,BenefitTypeId,IsCovered,LimitAmount,ExcessAmount,Remarks,IsActive,CreatedDate,CreatedBy")] PlanBenefits planBenefit)
        {
            if (id != planBenefit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    planBenefit.ModifiedDate = DateTime.UtcNow;
                    planBenefit.ModifiedBy = User.Identity?.Name;

                    _context.Update(planBenefit);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Plan benefit insurance updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanBenefitsExists(planBenefit.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(planBenefit.InsurancePlanId, planBenefit.BenefitTypeId);
            return View("~/Views/PlanBenefitInsurances/Edit.cshtml", planBenefit);
        }

        // GET: PlanBenefits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            // Project explicitly to avoid SqlNullValueException from nullable DB columns
            var planBenefit = await _context.PlanBenefits
                .AsNoTracking()
                .Where(pbi => pbi.Id == id)
                .Select(pbi => new PlanBenefits
                {
                    Id = pbi.Id,
                    InsurancePlanId = pbi.InsurancePlanId,
                    BenefitTypeId = pbi.BenefitTypeId,
                    IsCovered = pbi.IsCovered,
                    LimitAmount = pbi.LimitAmount,
                    ExcessAmount = pbi.ExcessAmount,
                    Remarks = pbi.Remarks,
                    IsActive = pbi.IsActive,
                    CreatedDate = pbi.CreatedDate,
                    ModifiedDate = pbi.ModifiedDate,

                    InsurancePlan = pbi.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pbi.InsurancePlan.Id,
                            PlanName = pbi.InsurancePlan.PlanName,
                            PlanNameAr = pbi.InsurancePlan.PlanNameAr
                        },

                    BenefitType = pbi.BenefitType == null
                        ? null
                        : new BenefitType
                        {
                            Id = pbi.BenefitType.Id,
                            BenefitTypeName = pbi.BenefitType.BenefitTypeName,
                            BenefitTypeNameAr = pbi.BenefitType.BenefitTypeNameAr
                        }
                })
                .FirstOrDefaultAsync();

            if (planBenefit == null)
                return NotFound();

            return View("~/Views/PlanBenefitInsurances/Delete.cshtml", planBenefit);
        }

        // POST: PlanBenefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planBenefit = await _context.PlanBenefits.FindAsync(id);
            if (planBenefit != null)
            {
                _context.PlanBenefits.Remove(planBenefit);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan benefit insurance deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanBenefitsExists(int id)
        {
            return _context.PlanBenefits.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedBenefitTypeId = null)
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

            // Benefit Types
            var benefitTypes = await _context.BenefitTypes
                .OrderBy(b => b.BenefitTypeName)
                .ToListAsync();

            ViewBag.BenefitTypes = new SelectList(
                benefitTypes,
                "Id",
                currentLang == "ar"
                    ? nameof(BenefitType.BenefitTypeNameAr)
                    : nameof(BenefitType.BenefitTypeName),
                selectedBenefitTypeId);
        }
    }
}