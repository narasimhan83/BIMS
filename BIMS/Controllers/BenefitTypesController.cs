using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class BenefitTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BenefitTypesController> _logger;

        public BenefitTypesController(ApplicationDbContext context, ILogger<BenefitTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: BenefitTypes
        public async Task<IActionResult> Index()
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var items = await _context.BenefitTypes
                .AsNoTracking()
                .OrderByDescending(b => b.CreatedDate)
                .Select(b => new BenefitType
                {
                    Id = b.Id,
                    VehicleCategoryId = b.VehicleCategoryId,
                    BenefitTypeName = b.BenefitTypeName,
                    BenefitTypeNameAr = b.BenefitTypeNameAr,
                    IsActive = b.IsActive,
                    CreatedDate = b.CreatedDate,
                    ModifiedDate = b.ModifiedDate,
                    VehicleCategory = b.VehicleCategory == null
                        ? null
                        : new VehicleCategory
                        {
                            Id = b.VehicleCategory.Id,
                            CategoryName = b.VehicleCategory.CategoryName,
                            CategoryNameAr = b.VehicleCategory.CategoryNameAr,
                            Code = b.VehicleCategory.Code
                        }
                })
                .ToListAsync();

            return View(items);
        }

        // GET: BenefitTypes/Create
        public async Task<IActionResult> Create()
        {
            await PopulateVehicleCategoriesDropdown();
            return View();
        }

        // POST: BenefitTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleCategoryId,BenefitTypeName,BenefitTypeNameAr,IsActive")] BenefitType benefitType)
        {
            if (ModelState.IsValid)
            {
                benefitType.CreatedDate = DateTime.UtcNow;
                benefitType.CreatedBy = User.Identity?.Name;

                _context.Add(benefitType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Benefit type created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateVehicleCategoriesDropdown(benefitType.VehicleCategoryId);
            return View(benefitType);
        }

        // GET: BenefitTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var benefitType = await _context.BenefitTypes.FindAsync(id);
            if (benefitType == null)
                return NotFound();

            await PopulateVehicleCategoriesDropdown(benefitType.VehicleCategoryId);
            return View(benefitType);
        }

        // POST: BenefitTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleCategoryId,BenefitTypeName,BenefitTypeNameAr,IsActive,CreatedDate,CreatedBy")] BenefitType benefitType)
        {
            if (id != benefitType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    benefitType.ModifiedDate = DateTime.UtcNow;
                    benefitType.ModifiedBy = User.Identity?.Name;

                    _context.Update(benefitType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Benefit type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenefitTypeExists(benefitType.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateVehicleCategoriesDropdown(benefitType.VehicleCategoryId);
            return View(benefitType);
        }

        // GET: BenefitTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var benefitType = await _context.BenefitTypes
                .AsNoTracking()
                .Include(b => b.VehicleCategory)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (benefitType == null)
                return NotFound();

            return View(benefitType);
        }

        // POST: BenefitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benefitType = await _context.BenefitTypes.FindAsync(id);
            if (benefitType != null)
            {
                _context.BenefitTypes.Remove(benefitType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Benefit type deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BenefitTypeExists(int id)
        {
            return _context.BenefitTypes.Any(e => e.Id == id);
        }

        private async Task PopulateVehicleCategoriesDropdown(int? selectedVehicleCategoryId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var categories = await _context.VehicleCategories
                .Where(vc => vc.IsActive)
                .OrderBy(vc => vc.CategoryName)
                .ToListAsync();

            ViewBag.VehicleCategories = new SelectList(
                categories,
                "Id",
                currentLang == "ar"
                    ? nameof(VehicleCategory.CategoryNameAr)
                    : nameof(VehicleCategory.CategoryName),
                selectedVehicleCategoryId);
        }
    }
}