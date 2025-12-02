using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanAdditionalCoversController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanAdditionalCoversController> _logger;

        public PlanAdditionalCoversController(ApplicationDbContext context, ILogger<PlanAdditionalCoversController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PlanAdditionalCovers
        public async Task<IActionResult> Index()
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var items = await _context.PlanAdditionalCovers
                .AsNoTracking()
                .OrderByDescending(pac => pac.CreatedDate)
                .Select(pac => new PlanAdditionalCover
                {
                    Id = pac.Id,
                    InsurancePlanId = pac.InsurancePlanId,
                    AdditionalCoverId = pac.AdditionalCoverId,
                    PremiumFixed = pac.PremiumFixed,
                    PremiumPercentage = pac.PremiumPercentage,
                    IsActive = pac.IsActive,
                    CreatedDate = pac.CreatedDate,
                    ModifiedDate = pac.ModifiedDate,

                    InsurancePlan = pac.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pac.InsurancePlan.Id,
                            PlanName = pac.InsurancePlan.PlanName,
                            PlanNameAr = pac.InsurancePlan.PlanNameAr
                        },

                    AdditionalCover = pac.AdditionalCover == null
                        ? null
                        : new AdditionalCover
                        {
                            Id = pac.AdditionalCover.Id,
                            CoverCode = pac.AdditionalCover.CoverCode,
                            CoverName = pac.AdditionalCover.CoverName,
                            CoverNameAr = pac.AdditionalCover.CoverNameAr,
                            IsActive = pac.AdditionalCover.IsActive
                        }
                })
                .ToListAsync();

            return View("~/Views/PlanAdditionalCovers/Index.cshtml", items);
        }

        // GET: PlanAdditionalCovers/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/PlanAdditionalCovers/Create.cshtml");
        }

        // POST: PlanAdditionalCovers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,AdditionalCoverId,PremiumFixed,PremiumPercentage,IsActive")] PlanAdditionalCover model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan additional cover created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.AdditionalCoverId);
            return View("~/Views/PlanAdditionalCovers/Create.cshtml", model);
        }

        // GET: PlanAdditionalCovers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanAdditionalCovers.FindAsync(id);
            if (entity == null)
                return NotFound();

            await PopulateDropdowns(entity.InsurancePlanId, entity.AdditionalCoverId);
            return View("~/Views/PlanAdditionalCovers/Edit.cshtml", entity);
        }

        // POST: PlanAdditionalCovers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,AdditionalCoverId,PremiumFixed,PremiumPercentage,IsActive,CreatedDate,CreatedBy")] PlanAdditionalCover model)
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
                    TempData["SuccessMessage"] = "Plan additional cover updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanAdditionalCoverExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.AdditionalCoverId);
            return View("~/Views/PlanAdditionalCovers/Edit.cshtml", model);
        }

        // GET: PlanAdditionalCovers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanAdditionalCovers
                .AsNoTracking()
                .Include(pac => pac.InsurancePlan)
                .Include(pac => pac.AdditionalCover)
                .FirstOrDefaultAsync(pac => pac.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/PlanAdditionalCovers/Delete.cshtml", entity);
        }

        // POST: PlanAdditionalCovers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.PlanAdditionalCovers.FindAsync(id);
            if (entity != null)
            {
                _context.PlanAdditionalCovers.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan additional cover deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanAdditionalCoverExists(int id)
        {
            return _context.PlanAdditionalCovers.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedAdditionalCoverId = null)
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

            // Additional Covers
            var covers = await _context.AdditionalCovers
                .OrderBy(c => c.CoverCode)
                .ThenBy(c => c.CoverName)
                .ToListAsync();

            ViewBag.AdditionalCovers = new SelectList(
                covers,
                "Id",
                currentLang == "ar"
                    ? nameof(AdditionalCover.CoverNameAr)
                    : nameof(AdditionalCover.CoverName),
                selectedAdditionalCoverId);
        }
    }
}