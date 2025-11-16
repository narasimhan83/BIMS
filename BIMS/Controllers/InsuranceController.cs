using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InsuranceController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InsuranceController(
            ApplicationDbContext context, 
            ILogger<InsuranceController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // ============================================
        // PRODUCT CLASS CRU Operations
        // ============================================

        // GET: Insurance/ProductClasses
        public async Task<IActionResult> ProductClasses()
        {
            var productClasses = await _context.ProductClasses
                .Include(pc => pc.BusinessType)
                .Include(pc => pc.ProductTypes)
                .OrderByDescending(pc => pc.CreatedDate)
                .ToListAsync();
            return View(productClasses);
        }

        // GET: Insurance/CreateProductClass
        public IActionResult CreateProductClass()
        {
            PopulateBusinessTypesDropdown();
            return View();
        }

        // POST: Insurance/CreateProductClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductClass(ProductClass productClass, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImageAsync(imageFile, "product-classes");
                    if (imagePath != null)
                    {
                        productClass.ImagePath = imagePath;
                    }
                }

                productClass.CreatedDate = DateTime.UtcNow;
                productClass.CreatedBy = User.Identity?.Name;
                
                _context.Add(productClass);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product Class created successfully!";
                return RedirectToAction(nameof(ProductClasses));
            }
            
            PopulateBusinessTypesDropdown(productClass.BusinessTypeId);
            return View(productClass);
        }

        // GET: Insurance/EditProductClass/5
        public async Task<IActionResult> EditProductClass(int? id)
        {
            if (id == null) return NotFound();

            var productClass = await _context.ProductClasses.FindAsync(id);
            if (productClass == null) return NotFound();

            PopulateBusinessTypesDropdown(productClass.BusinessTypeId);
            return View(productClass);
        }

        // POST: Insurance/EditProductClass/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductClass(int id, ProductClass productClass, IFormFile? imageFile, bool deleteImage = false)
        {
            if (id != productClass.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProductClass = await _context.ProductClasses.FindAsync(id);
                    if (existingProductClass == null) return NotFound();

                    // Handle image deletion
                    if (deleteImage && !string.IsNullOrEmpty(existingProductClass.ImagePath))
                    {
                        DeleteImage(existingProductClass.ImagePath);
                        existingProductClass.ImagePath = null;
                    }

                    // Handle new image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingProductClass.ImagePath))
                        {
                            DeleteImage(existingProductClass.ImagePath);
                        }

                        var imagePath = await SaveImageAsync(imageFile, "product-classes");
                        if (imagePath != null)
                        {
                            existingProductClass.ImagePath = imagePath;
                        }
                    }

                    // Update properties
                    existingProductClass.BusinessTypeId = productClass.BusinessTypeId;
                    existingProductClass.ProductClassName = productClass.ProductClassName;
                    existingProductClass.ProductClassNameAr = productClass.ProductClassNameAr;
                    existingProductClass.Code = productClass.Code;
                    existingProductClass.Description = productClass.Description;
                    existingProductClass.DescriptionAr = productClass.DescriptionAr;
                    existingProductClass.IsActive = productClass.IsActive;
                    existingProductClass.ModifiedDate = DateTime.UtcNow;
                    existingProductClass.ModifiedBy = User.Identity?.Name;

                    _context.Update(existingProductClass);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product Class updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductClassExists(productClass.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(ProductClasses));
            }
            
            PopulateBusinessTypesDropdown(productClass.BusinessTypeId);
            return View(productClass);
        }

        // GET: Insurance/DeleteProductClass/5
        public async Task<IActionResult> DeleteProductClass(int? id)
        {
            if (id == null) return NotFound();

            var productClass = await _context.ProductClasses
                .Include(pc => pc.BusinessType)
                .Include(pc => pc.ProductTypes)
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (productClass == null) return NotFound();

            return View(productClass);
        }

        // POST: Insurance/DeleteProductClassConfirmed/5
        [HttpPost, ActionName("DeleteProductClass")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductClassConfirmed(int id)
        {
            var productClass = await _context.ProductClasses
                .Include(pc => pc.ProductTypes)
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (productClass != null)
            {
                // Check for dependent Product Types
                if (productClass.ProductTypes?.Any() == true)
                {
                    TempData["ErrorMessage"] = "Cannot delete Product Class with associated Product Types. Delete Product Types first.";
                    return RedirectToAction(nameof(ProductClasses));
                }

                // Delete image if exists
                if (!string.IsNullOrEmpty(productClass.ImagePath))
                {
                    DeleteImage(productClass.ImagePath);
                }

                _context.ProductClasses.Remove(productClass);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product Class deleted successfully!";
            }

            return RedirectToAction(nameof(ProductClasses));
        }

        private bool ProductClassExists(int id)
        {
            return _context.ProductClasses.Any(e => e.Id == id);
        }

        // ============================================
        // PRODUCT TYPE CRUD OPERATIONS
        // ============================================

        // GET: Insurance/ProductTypes
        public async Task<IActionResult> ProductTypes()
        {
            var productTypes = await _context.ProductTypes
                .Include(pt => pt.ProductClass)
                    .ThenInclude(pc => pc!.BusinessType)
                .OrderByDescending(pt => pt.CreatedDate)
                .ToListAsync();
            return View(productTypes);
        }

        // GET: Insurance/CreateProductType
        public IActionResult CreateProductType()
        {
            PopulateProductClassesDropdown();
            return View();
        }

        // POST: Insurance/CreateProductType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductType(ProductType productType, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImageAsync(imageFile, "product-types");
                    if (imagePath != null)
                    {
                        productType.ImagePath = imagePath;
                    }
                }

                productType.CreatedDate = DateTime.UtcNow;
                productType.CreatedBy = User.Identity?.Name;
                
                _context.Add(productType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product Type created successfully!";
                return RedirectToAction(nameof(ProductTypes));
            }
            
            PopulateProductClassesDropdown(productType.ProductClassId);
            return View(productType);
        }

        // GET: Insurance/EditProductType/5
        public async Task<IActionResult> EditProductType(int? id)
        {
            if (id == null) return NotFound();

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null) return NotFound();

            PopulateProductClassesDropdown(productType.ProductClassId);
            return View(productType);
        }

        // POST: Insurance/EditProductType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductType(int id, ProductType productType, IFormFile? imageFile, bool deleteImage = false)
        {
            if (id != productType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProductType = await _context.ProductTypes.FindAsync(id);
                    if (existingProductType == null) return NotFound();

                    // Handle image deletion
                    if (deleteImage && !string.IsNullOrEmpty(existingProductType.ImagePath))
                    {
                        DeleteImage(existingProductType.ImagePath);
                        existingProductType.ImagePath = null;
                    }

                    // Handle new image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingProductType.ImagePath))
                        {
                            DeleteImage(existingProductType.ImagePath);
                        }

                        var imagePath = await SaveImageAsync(imageFile, "product-types");
                        if (imagePath != null)
                        {
                            existingProductType.ImagePath = imagePath;
                        }
                    }

                    // Update properties
                    existingProductType.ProductClassId = productType.ProductClassId;
                    existingProductType.ProductTypeName = productType.ProductTypeName;
                    existingProductType.ProductTypeNameAr = productType.ProductTypeNameAr;
                    existingProductType.Code = productType.Code;
                    existingProductType.Description = productType.Description;
                    existingProductType.DescriptionAr = productType.DescriptionAr;
                    existingProductType.IsActive = productType.IsActive;
                    existingProductType.ModifiedDate = DateTime.UtcNow;
                    existingProductType.ModifiedBy = User.Identity?.Name;

                    _context.Update(existingProductType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(ProductTypes));
            }
            
            PopulateProductClassesDropdown(productType.ProductClassId);
            return View(productType);
        }

        // GET: Insurance/DeleteProductType/5
        public async Task<IActionResult> DeleteProductType(int? id)
        {
            if (id == null) return NotFound();

            var productType = await _context.ProductTypes
                .Include(pt => pt.ProductClass)
                    .ThenInclude(pc => pc!.BusinessType)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (productType == null) return NotFound();

            return View(productType);
        }

        // POST: Insurance/DeleteProductTypeConfirmed/5
        [HttpPost, ActionName("DeleteProductType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductTypeConfirmed(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);

            if (productType != null)
            {
                // Delete image if exists
                if (!string.IsNullOrEmpty(productType.ImagePath))
                {
                    DeleteImage(productType.ImagePath);
                }

                _context.ProductTypes.Remove(productType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product Type deleted successfully!";
            }

            return RedirectToAction(nameof(ProductTypes));
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.Id == id);
        }

        // ============================================
        // API ENDPOINTS
        // ============================================

        // API: Get Product Classes by Business Type
        [HttpGet]
        public async Task<IActionResult> GetProductClassesByBusinessType(int businessTypeId)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var productClasses = await _context.ProductClasses
                .Where(pc => pc.BusinessTypeId == businessTypeId && pc.IsActive)
                .Select(pc => new
                {
                    Id = pc.Id,
                    ProductClassName = currentLang == "ar" && !string.IsNullOrEmpty(pc.ProductClassNameAr)
                        ? pc.ProductClassNameAr
                        : pc.ProductClassName
                })
                .ToListAsync();
            
            return Json(productClasses);
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        private void PopulateBusinessTypesDropdown(object? selectedValue = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var businessTypes = _context.BusinessTypes
                .Where(bt => bt.IsActive)
                .Select(bt => new
                {
                    bt.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(bt.NameAr)
                        ? bt.NameAr
                        : bt.Name
                })
                .ToList();

            ViewBag.BusinessTypes = new SelectList(businessTypes, "Id", "DisplayName", selectedValue);
        }

        private void PopulateProductClassesDropdown(object? selectedValue = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var productClasses = _context.ProductClasses
                .Where(pc => pc.IsActive)
                .Include(pc => pc.BusinessType)
                .Select(pc => new
                {
                    pc.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(pc.ProductClassNameAr)
                        ? $"{pc.ProductClassNameAr} ({pc.BusinessType!.Name})"
                        : $"{pc.ProductClassName} ({pc.BusinessType!.Name})"
                })
                .ToList();

            ViewBag.ProductClasses = new SelectList(productClasses, "Id", "DisplayName", selectedValue);
        }

        // ============================================
        // IMAGE MANAGEMENT
        // ============================================

        private async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            try
            {
                // Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    TempData["ErrorMessage"] = "Invalid image format. Allowed: JPG, PNG, GIF, WEBP";
                    return null;
                }

                // Check file size (2MB)
                if (file.Length > 2 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Image size must be less than 2MB";
                    return null;
                }

                // Create upload directory
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "insurance", folder);
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for database
                return $"/uploads/insurance/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image");
                TempData["ErrorMessage"] = $"Error uploading image: {ex.Message}";
                return null;
            }
        }

        private void DeleteImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            try
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {ImagePath}", imagePath);
            }
        }
    }
}