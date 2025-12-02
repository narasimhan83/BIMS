using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class SpecialConditionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SpecialConditionsController> _logger;

        public SpecialConditionsController(ApplicationDbContext context, ILogger<SpecialConditionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: SpecialConditions
        public async Task<IActionResult> Index()
        {
            var items = await _context.SpecialConditions
                .AsNoTracking()
                .OrderByDescending(sc => sc.CreatedDate)
                .ToListAsync();

            return View("~/Views/SpecialConditions/Index.cshtml", items);
        }

        // GET: SpecialConditions/Create
        public IActionResult Create()
        {
            return View("~/Views/SpecialConditions/Create.cshtml");
        }

        // POST: SpecialConditions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConditionCode,Description,IsActive")] SpecialCondition model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Special condition created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/SpecialConditions/Create.cshtml", model);
        }

        // GET: SpecialConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.SpecialConditions.FindAsync(id);
            if (entity == null)
                return NotFound();

            return View("~/Views/SpecialConditions/Edit.cshtml", entity);
        }

        // POST: SpecialConditions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConditionCode,Description,IsActive,CreatedDate,CreatedBy")] SpecialCondition model)
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
                    TempData["SuccessMessage"] = "Special condition updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialConditionExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/SpecialConditions/Edit.cshtml", model);
        }

        // GET: SpecialConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.SpecialConditions
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/SpecialConditions/Delete.cshtml", entity);
        }

        // POST: SpecialConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.SpecialConditions.FindAsync(id);
            if (entity != null)
            {
                _context.SpecialConditions.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Special condition deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SpecialConditionExists(int id)
        {
            return _context.SpecialConditions.Any(e => e.Id == id);
        }
    }
}