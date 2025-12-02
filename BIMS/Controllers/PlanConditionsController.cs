using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanConditionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanConditionsController> _logger;

        public PlanConditionsController(ApplicationDbContext context, ILogger<PlanConditionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PlanConditions
        public async Task<IActionResult> Index()
        {
            var items = await _context.PlanConditions
                .AsNoTracking()
                .OrderByDescending(pc => pc.CreatedDate)
                .Select(pc => new PlanCondition
                {
                    Id = pc.Id,
                    InsurancePlanId = pc.InsurancePlanId,
                    SpecialConditionId = pc.SpecialConditionId,
                    LoadingPercentage = pc.LoadingPercentage,
                    AppliesWhen = pc.AppliesWhen,
                    IsActive = pc.IsActive,
                    CreatedDate = pc.CreatedDate,
                    ModifiedDate = pc.ModifiedDate,

                    InsurancePlan = pc.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pc.InsurancePlan.Id,
                            PlanName = pc.InsurancePlan.PlanName,
                            PlanNameAr = pc.InsurancePlan.PlanNameAr
                        },

                    SpecialCondition = pc.SpecialCondition == null
                        ? null
                        : new SpecialCondition
                        {
                            Id = pc.SpecialCondition.Id,
                            ConditionCode = pc.SpecialCondition.ConditionCode,
                            Description = pc.SpecialCondition.Description,
                            IsActive = pc.SpecialCondition.IsActive
                        }
                })
                .ToListAsync();

            return View("~/Views/PlanConditions/Index.cshtml", items);
        }

        // GET: PlanConditions/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/PlanConditions/Create.cshtml");
        }

        // POST: PlanConditions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,SpecialConditionId,LoadingPercentage,AppliesWhen,IsActive")] PlanCondition model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan condition created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.SpecialConditionId);
            return View("~/Views/PlanConditions/Create.cshtml", model);
        }

        // GET: PlanConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanConditions.FindAsync(id);
            if (entity == null)
                return NotFound();

            await PopulateDropdowns(entity.InsurancePlanId, entity.SpecialConditionId);
            return View("~/Views/PlanConditions/Edit.cshtml", entity);
        }

        // POST: PlanConditions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,SpecialConditionId,LoadingPercentage,AppliesWhen,IsActive,CreatedDate,CreatedBy")] PlanCondition model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifiedDate = DateTime.UtcNow;
                    model.ModifiedBy = User.Identity?.Name;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Plan condition updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanConditionExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.SpecialConditionId);
            return View("~/Views/PlanConditions/Edit.cshtml", model);
        }

        // GET: PlanConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanConditions
                .AsNoTracking()
                .Include(pc => pc.InsurancePlan)
                .Include(pc => pc.SpecialCondition)
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/PlanConditions/Delete.cshtml", entity);
        }

        // POST: PlanConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.PlanConditions.FindAsync(id);
            if (entity != null)
            {
                _context.PlanConditions.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan condition deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanConditionExists(int id)
        {
            return _context.PlanConditions.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedSpecialConditionId = null)
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

            // Special Conditions
            var specialConditions = await _context.SpecialConditions
                .OrderBy(sc => sc.ConditionCode)
                .ThenBy(sc => sc.Description)
                .ToListAsync();

            ViewBag.SpecialConditions = new SelectList(
                specialConditions,
                "Id",
                nameof(SpecialCondition.Description),
                selectedSpecialConditionId);
        }
    }
}