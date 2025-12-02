using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class RenewalRulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RenewalRulesController> _logger;

        public RenewalRulesController(ApplicationDbContext context, ILogger<RenewalRulesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: RenewalRules
        public async Task<IActionResult> Index()
        {
            var items = await _context.RenewalRules
                .AsNoTracking()
                .OrderByDescending(rr => rr.CreatedDate)
                .Select(rr => new RenewalRule
                {
                    Id = rr.Id,
                    InsurancePlanId = rr.InsurancePlanId,
                    LineOfBusinessId = rr.LineOfBusinessId,
                    ClaimsCount = rr.ClaimsCount,
                    LoadingPercentage = rr.LoadingPercentage,
                    MinimumPremium = rr.MinimumPremium,
                    Action = rr.Action,
                    EffectiveYear = rr.EffectiveYear,
                    IsActive = rr.IsActive,
                    CreatedDate = rr.CreatedDate,
                    ModifiedDate = rr.ModifiedDate,

                    InsurancePlan = rr.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = rr.InsurancePlan.Id,
                            PlanName = rr.InsurancePlan.PlanName,
                            PlanNameAr = rr.InsurancePlan.PlanNameAr
                        },

                    LineOfBusiness = rr.LineOfBusiness == null
                        ? null
                        : new LineOfBusiness
                        {
                            Id = rr.LineOfBusiness.Id,
                            Name = rr.LineOfBusiness.Name,
                            NameAr = rr.LineOfBusiness.NameAr
                        }
                })
                .ToListAsync();

            return View("~/Views/RenewalRules/Index.cshtml", items);
        }

        // GET: RenewalRules/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/RenewalRules/Create.cshtml");
        }

        // POST: RenewalRules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,LineOfBusinessId,ClaimsCount,LoadingPercentage,MinimumPremium,Action,EffectiveYear,IsActive")] RenewalRule model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Renewal rule created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.LineOfBusinessId);
            return View("~/Views/RenewalRules/Create.cshtml", model);
        }

        // GET: RenewalRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.RenewalRules.FindAsync(id);
            if (entity == null)
                return NotFound();

            await PopulateDropdowns(entity.InsurancePlanId, entity.LineOfBusinessId);
            return View("~/Views/RenewalRules/Edit.cshtml", entity);
        }

        // POST: RenewalRules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,LineOfBusinessId,ClaimsCount,LoadingPercentage,MinimumPremium,Action,EffectiveYear,IsActive,CreatedDate,CreatedBy")] RenewalRule model)
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
                    TempData["SuccessMessage"] = "Renewal rule updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RenewalRuleExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.LineOfBusinessId);
            return View("~/Views/RenewalRules/Edit.cshtml", model);
        }

        // GET: RenewalRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.RenewalRules
                .AsNoTracking()
                .Include(rr => rr.InsurancePlan)
                .Include(rr => rr.LineOfBusiness)
                .FirstOrDefaultAsync(rr => rr.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/RenewalRules/Delete.cshtml", entity);
        }

        // POST: RenewalRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.RenewalRules.FindAsync(id);
            if (entity != null)
            {
                _context.RenewalRules.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Renewal rule deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RenewalRuleExists(int id)
        {
            return _context.RenewalRules.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedLobId = null)
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

            // Lines of Business
            var lobs = await _context.LinesOfBusiness
                .OrderBy(l => l.Name)
                .ToListAsync();

            ViewBag.LinesOfBusiness = new SelectList(
                lobs,
                "Id",
                currentLang == "ar"
                    ? nameof(LineOfBusiness.NameAr)
                    : nameof(LineOfBusiness.Name),
                selectedLobId);
        }
    }
}