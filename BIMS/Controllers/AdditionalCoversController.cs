using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class AdditionalCoversController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdditionalCoversController> _logger;

        public AdditionalCoversController(ApplicationDbContext context, ILogger<AdditionalCoversController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: AdditionalCovers
        public async Task<IActionResult> Index()
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var items = await _context.AdditionalCovers
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View("~/Views/AdditionalCovers/Index.cshtml", items);
        }

        // GET: AdditionalCovers/Create
        public IActionResult Create()
        {
            return View("~/Views/AdditionalCovers/Create.cshtml");
        }

        // POST: AdditionalCovers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoverCode,CoverName,CoverNameAr,IsActive")] AdditionalCover cover)
        {
            if (ModelState.IsValid)
            {
                cover.CreatedDate = DateTime.UtcNow;
                cover.CreatedBy = User.Identity?.Name;

                _context.Add(cover);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Additional cover created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/AdditionalCovers/Create.cshtml", cover);
        }

        // GET: AdditionalCovers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cover = await _context.AdditionalCovers.FindAsync(id);
            if (cover == null)
                return NotFound();

            return View("~/Views/AdditionalCovers/Edit.cshtml", cover);
        }

        // POST: AdditionalCovers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoverCode,CoverName,CoverNameAr,IsActive,CreatedDate,CreatedBy")] AdditionalCover cover)
        {
            if (id != cover.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    cover.ModifiedDate = DateTime.UtcNow;
                    cover.ModifiedBy = User.Identity?.Name;

                    _context.Update(cover);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Additional cover updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdditionalCoverExists(cover.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/AdditionalCovers/Edit.cshtml", cover);
        }

        // GET: AdditionalCovers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cover = await _context.AdditionalCovers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cover == null)
                return NotFound();

            return View("~/Views/AdditionalCovers/Delete.cshtml", cover);
        }

        // POST: AdditionalCovers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cover = await _context.AdditionalCovers.FindAsync(id);
            if (cover != null)
            {
                _context.AdditionalCovers.Remove(cover);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Additional cover deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AdditionalCoverExists(int id)
        {
            return _context.AdditionalCovers.Any(e => e.Id == id);
        }
    }
}