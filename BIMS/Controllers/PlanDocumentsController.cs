using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanDocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanDocumentsController> _logger;

        public PlanDocumentsController(ApplicationDbContext context, ILogger<PlanDocumentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PlanDocuments
        public async Task<IActionResult> Index()
        {
            var items = await _context.PlanDocuments
                .AsNoTracking()
                .OrderByDescending(pd => pd.CreatedDate)
                .Select(pd => new PlanDocument
                {
                    Id = pd.Id,
                    InsurancePlanId = pd.InsurancePlanId,
                    DocumentUrl = pd.DocumentUrl,
                    IsActive = pd.IsActive,
                    CreatedDate = pd.CreatedDate,
                    ModifiedDate = pd.ModifiedDate,
                    InsurancePlan = pd.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pd.InsurancePlan.Id,
                            PlanName = pd.InsurancePlan.PlanName,
                            PlanNameAr = pd.InsurancePlan.PlanNameAr
                        }
                })
                .ToListAsync();

            return View("~/Views/PlanDocuments/Index.cshtml", items);
        }

        // GET: PlanDocuments/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/PlanDocuments/Create.cshtml");
        }

        // POST: PlanDocuments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,DocumentUrl,IsActive")] PlanDocument model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan document created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId);
            return View("~/Views/PlanDocuments/Create.cshtml", model);
        }

        // GET: PlanDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanDocuments.FindAsync(id);
            if (entity == null)
                return NotFound();

            await PopulateDropdowns(entity.InsurancePlanId);
            return View("~/Views/PlanDocuments/Edit.cshtml", entity);
        }

        // POST: PlanDocuments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,DocumentUrl,IsActive,CreatedDate,CreatedBy")] PlanDocument model)
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
                    TempData["SuccessMessage"] = "Plan document updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanDocumentExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId);
            return View("~/Views/PlanDocuments/Edit.cshtml", model);
        }

        // GET: PlanDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanDocuments
                .AsNoTracking()
                .Include(pd => pd.InsurancePlan)
                .FirstOrDefaultAsync(pd => pd.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/PlanDocuments/Delete.cshtml", entity);
        }

        // POST: PlanDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.PlanDocuments.FindAsync(id);
            if (entity != null)
            {
                _context.PlanDocuments.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan document deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanDocumentExists(int id)
        {
            return _context.PlanDocuments.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(int? selectedPlanId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

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
        }
    }
}