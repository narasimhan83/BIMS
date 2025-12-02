using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class CoverageCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CoverageCategoriesController> _logger;

        public CoverageCategoriesController(ApplicationDbContext context, ILogger<CoverageCategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: CoverageCategories
        public async Task<IActionResult> Index()
        {
            var items = await _context.CoverageCategories
                .AsNoTracking()
                .OrderByDescending(cc => cc.CreatedDate)
                .Select(cc => new CoverageCategory
                {
                    Id = cc.Id,
                    ProductClassId = cc.ProductClassId,
                    ProductTypeId = cc.ProductTypeId,
                    VehicleCategoryId = cc.VehicleCategoryId,
                    IsActive = cc.IsActive,
                    CreatedDate = cc.CreatedDate,

                    ProductClass = cc.ProductClass == null
                        ? null
                        : new ProductClass
                        {
                            Id = cc.ProductClass.Id,
                            ProductClassName = cc.ProductClass.ProductClassName,
                            ProductClassNameAr = cc.ProductClass.ProductClassNameAr
                        },

                    ProductType = cc.ProductType == null
                        ? null
                        : new ProductType
                        {
                            Id = cc.ProductType.Id,
                            ProductTypeName = cc.ProductType.ProductTypeName,
                            ProductTypeNameAr = cc.ProductType.ProductTypeNameAr
                        },

                    VehicleCategory = cc.VehicleCategory == null
                        ? null
                        : new VehicleCategory
                        {
                            Id = cc.VehicleCategory.Id,
                            CategoryName = cc.VehicleCategory.CategoryName,
                            CategoryNameAr = cc.VehicleCategory.CategoryNameAr
                        }
                })
                .ToListAsync();

            return View(items);
        }

        // GET: CoverageCategories/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: CoverageCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductClassId,ProductTypeId,VehicleCategoryId,IsActive")] CoverageCategory coverageCategory)
        {
            if (ModelState.IsValid)
            {
                coverageCategory.CreatedDate = DateTime.UtcNow;
                coverageCategory.CreatedBy = User.Identity?.Name;

                _context.Add(coverageCategory);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Coverage category created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(coverageCategory.ProductClassId, coverageCategory.ProductTypeId, coverageCategory.VehicleCategoryId);
            return View(coverageCategory);
        }

        // GET: CoverageCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverageCategory = await _context.CoverageCategories.FindAsync(id);
            if (coverageCategory == null)
            {
                return NotFound();
            }

            await PopulateDropdowns(coverageCategory.ProductClassId, coverageCategory.ProductTypeId, coverageCategory.VehicleCategoryId);
            return View(coverageCategory);
        }

        // POST: CoverageCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductClassId,ProductTypeId,VehicleCategoryId,IsActive,CreatedDate,CreatedBy")] CoverageCategory coverageCategory)
        {
            if (id != coverageCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    coverageCategory.ModifiedDate = DateTime.UtcNow;
                    coverageCategory.ModifiedBy = User.Identity?.Name;

                    _context.Update(coverageCategory);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Coverage category updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoverageCategoryExists(coverageCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(coverageCategory.ProductClassId, coverageCategory.ProductTypeId, coverageCategory.VehicleCategoryId);
            return View(coverageCategory);
        }

        // GET: CoverageCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverageCategory = await _context.CoverageCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (coverageCategory == null)
            {
                return NotFound();
            }

            return View(coverageCategory);
        }

        // POST: CoverageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coverageCategory = await _context.CoverageCategories.FindAsync(id);
            if (coverageCategory != null)
            {
                _context.CoverageCategories.Remove(coverageCategory);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Coverage category deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CoverageCategoryExists(int id)
        {
            return _context.CoverageCategories.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedProductClassId = null,
            int? selectedProductTypeId = null,
            int? selectedVehicleCategoryId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Product Classes
            var classes = await _context.ProductClasses
                .Where(pc => pc.IsActive)
                .OrderBy(pc => pc.ProductClassName)
                .ToListAsync();

            ViewBag.ProductClasses = new SelectList(
                classes,
                "Id",
                currentLang == "ar"
                    ? nameof(ProductClass.ProductClassNameAr)
                    : nameof(ProductClass.ProductClassName),
                selectedProductClassId);

            // Product Types
            var types = await _context.ProductTypes
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.ProductTypeName)
                .ToListAsync();

            ViewBag.ProductTypes = new SelectList(
                types,
                "Id",
                currentLang == "ar"
                    ? nameof(ProductType.ProductTypeNameAr)
                    : nameof(ProductType.ProductTypeName),
                selectedProductTypeId);

            // Vehicle Categories
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