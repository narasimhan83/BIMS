using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class ProviderNetworksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProviderNetworksController> _logger;

        public ProviderNetworksController(ApplicationDbContext context, ILogger<ProviderNetworksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ProviderNetworks
        public async Task<IActionResult> Index()
        {
            var items = await _context.ProviderNetworks
                .AsNoTracking()
                .OrderByDescending(pn => pn.CreatedDate)
                .ToListAsync();

            return View("~/Views/ProviderNetworks/Index.cshtml", items);
        }

        // GET: ProviderNetworks/Create
        public IActionResult Create()
        {
            return View("~/Views/ProviderNetworks/Create.cshtml");
        }

        // POST: ProviderNetworks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NetworkName,IsActive")] ProviderNetwork model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Provider network created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/ProviderNetworks/Create.cshtml", model);
        }

        // GET: ProviderNetworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.ProviderNetworks.FindAsync(id);
            if (entity == null)
                return NotFound();

            return View("~/Views/ProviderNetworks/Edit.cshtml", entity);
        }

        // POST: ProviderNetworks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NetworkName,IsActive,CreatedDate,CreatedBy")] ProviderNetwork model)
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
                    TempData["SuccessMessage"] = "Provider network updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderNetworkExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/ProviderNetworks/Edit.cshtml", model);
        }

        // GET: ProviderNetworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.ProviderNetworks
                .AsNoTracking()
                .FirstOrDefaultAsync(pn => pn.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/ProviderNetworks/Delete.cshtml", entity);
        }

        // POST: ProviderNetworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.ProviderNetworks.FindAsync(id);
            if (entity != null)
            {
                _context.ProviderNetworks.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Provider network deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProviderNetworkExists(int id)
        {
            return _context.ProviderNetworks.Any(e => e.Id == id);
        }
    }
}