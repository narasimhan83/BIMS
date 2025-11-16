using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ApplicationDbContext context, ILogger<VehicleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // VEHICLE MAKE CRUD OPERATIONS
        // ============================================

        // GET: Vehicle/VehicleMakes
        public async Task<IActionResult> VehicleMakes()
        {
            var makes = await _context.VehicleMakes
                .Include(m => m.VehicleModels)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();
            return View(makes);
        }

        // GET: Vehicle/CreateVehicleMake
        public IActionResult CreateVehicleMake()
        {
            return View();
        }

        // POST: Vehicle/CreateVehicleMake
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicleMake([Bind("MakeName,MakeNameAr,Code,Description,DescriptionAr,IsActive")] VehicleMake vehicleMake)
        {
            if (ModelState.IsValid)
            {
                vehicleMake.CreatedDate = DateTime.UtcNow;
                vehicleMake.CreatedBy = User.Identity?.Name;
                _context.Add(vehicleMake);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Make created successfully!";
                return RedirectToAction(nameof(VehicleMakes));
            }
            return View(vehicleMake);
        }

        // GET: Vehicle/EditVehicleMake/5
        public async Task<IActionResult> EditVehicleMake(int? id)
        {
            if (id == null) return NotFound();

            var vehicleMake = await _context.VehicleMakes.FindAsync(id);
            if (vehicleMake == null) return NotFound();

            return View(vehicleMake);
        }

        // POST: Vehicle/EditVehicleMake/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicleMake(int id, [Bind("Id,MakeName,MakeNameAr,Code,Description,DescriptionAr,IsActive,CreatedDate,CreatedBy")] VehicleMake vehicleMake)
        {
            if (id != vehicleMake.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleMake.ModifiedDate = DateTime.UtcNow;
                    vehicleMake.ModifiedBy = User.Identity?.Name;
                    _context.Update(vehicleMake);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle Make updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMakeExists(vehicleMake.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(VehicleMakes));
            }
            return View(vehicleMake);
        }

        // GET: Vehicle/DeleteVehicleMake/5
        public async Task<IActionResult> DeleteVehicleMake(int? id)
        {
            if (id == null) return NotFound();

            var vehicleMake = await _context.VehicleMakes
                .Include(m => m.VehicleModels)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (vehicleMake == null) return NotFound();

            return View(vehicleMake);
        }

        // POST: Vehicle/DeleteVehicleMakeConfirmed/5
        [HttpPost, ActionName("DeleteVehicleMake")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleMakeConfirmed(int id)
        {
            var vehicleMake = await _context.VehicleMakes
                .Include(m => m.VehicleModels)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (vehicleMake != null)
            {
                if (vehicleMake.VehicleModels?.Any() == true)
                {
                    TempData["ErrorMessage"] = "Cannot delete Vehicle Make with associated models. Delete models first.";
                    return RedirectToAction(nameof(VehicleMakes));
                }

                _context.VehicleMakes.Remove(vehicleMake);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Make deleted successfully!";
            }
            return RedirectToAction(nameof(VehicleMakes));
        }

        private bool VehicleMakeExists(int id)
        {
            return _context.VehicleMakes.Any(e => e.Id == id);
        }

        // ============================================
        // VEHICLE MODEL CRUD OPERATIONS
        // ============================================

        // GET: Vehicle/VehicleModels
        public async Task<IActionResult> VehicleModels()
        {
            var models = await _context.VehicleModels
                .Include(m => m.VehicleMake)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();
            return View(models);
        }

        // GET: Vehicle/CreateVehicleModel
        public IActionResult CreateVehicleModel()
        {
            PopulateMakesDropdown();
            return View();
        }

        // POST: Vehicle/CreateVehicleModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicleModel([Bind("VehicleMakeId,ModelName,ModelNameAr,Code,Description,DescriptionAr,IsActive")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                vehicleModel.CreatedDate = DateTime.UtcNow;
                vehicleModel.CreatedBy = User.Identity?.Name;
                _context.Add(vehicleModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Model created successfully!";
                return RedirectToAction(nameof(VehicleModels));
            }
            PopulateMakesDropdown(vehicleModel.VehicleMakeId);
            return View(vehicleModel);
        }

        // GET: Vehicle/EditVehicleModel/5
        public async Task<IActionResult> EditVehicleModel(int? id)
        {
            if (id == null) return NotFound();

            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel == null) return NotFound();

            PopulateMakesDropdown(vehicleModel.VehicleMakeId);
            return View(vehicleModel);
        }

        // POST: Vehicle/EditVehicleModel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicleModel(int id, [Bind("Id,VehicleMakeId,ModelName,ModelNameAr,Code,Description,DescriptionAr,IsActive,CreatedDate,CreatedBy")] VehicleModel vehicleModel)
        {
            if (id != vehicleModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleModel.ModifiedDate = DateTime.UtcNow;
                    vehicleModel.ModifiedBy = User.Identity?.Name;
                    _context.Update(vehicleModel);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle Model updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(VehicleModels));
            }
            PopulateMakesDropdown(vehicleModel.VehicleMakeId);
            return View(vehicleModel);
        }

        // GET: Vehicle/DeleteVehicleModel/5
        public async Task<IActionResult> DeleteVehicleModel(int? id)
        {
            if (id == null) return NotFound();

            var vehicleModel = await _context.VehicleModels
                .Include(m => m.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (vehicleModel == null) return NotFound();

            return View(vehicleModel);
        }

        // POST: Vehicle/DeleteVehicleModelConfirmed/5
        [HttpPost, ActionName("DeleteVehicleModel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleModelConfirmed(int id)
        {
            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel != null)
            {
                _context.VehicleModels.Remove(vehicleModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Model deleted successfully!";
            }
            return RedirectToAction(nameof(VehicleModels));
        }

        private bool VehicleModelExists(int id)
        {
            return _context.VehicleModels.Any(e => e.Id == id);
        }

        private void PopulateMakesDropdown(object? selectedMake = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var makes = _context.VehicleMakes
                .Where(m => m.IsActive)
                .OrderBy(m => m.MakeName)
                .Select(m => new
                {
                    m.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(m.MakeNameAr)
                        ? $"{m.MakeNameAr} ({m.MakeName})"
                        : m.MakeName
                })
                .ToList();

            ViewBag.VehicleMakes = new SelectList(makes, "Id", "DisplayName", selectedMake);
        }

        // ============================================
        // VEHICLE YEAR CRUD OPERATIONS
        // ============================================

        // GET: Vehicle/VehicleYears
        public async Task<IActionResult> VehicleYears()
        {
            var years = await _context.VehicleYears
                .OrderByDescending(y => y.Year)
                .ToListAsync();
            return View(years);
        }

        // GET: Vehicle/CreateVehicleYear
        public IActionResult CreateVehicleYear()
        {
            return View();
        }

        // POST: Vehicle/CreateVehicleYear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicleYear([Bind("Year,YearDisplay,YearDisplayAr,Description,DescriptionAr,IsActive")] VehicleYear vehicleYear)
        {
            if (ModelState.IsValid)
            {
                // Auto-generate display names if not provided
                if (string.IsNullOrEmpty(vehicleYear.YearDisplay))
                    vehicleYear.YearDisplay = $"{vehicleYear.Year} Model Year";
                
                if (string.IsNullOrEmpty(vehicleYear.YearDisplayAr))
                    vehicleYear.YearDisplayAr = $"موديل {vehicleYear.Year}";

                vehicleYear.CreatedDate = DateTime.UtcNow;
                _context.Add(vehicleYear);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Year created successfully!";
                return RedirectToAction(nameof(VehicleYears));
            }
            return View(vehicleYear);
        }

        // GET: Vehicle/EditVehicleYear/5
        public async Task<IActionResult> EditVehicleYear(int? id)
        {
            if (id == null) return NotFound();

            var vehicleYear = await _context.VehicleYears.FindAsync(id);
            if (vehicleYear == null) return NotFound();

            return View(vehicleYear);
        }

        // POST: Vehicle/EditVehicleYear/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicleYear(int id, [Bind("Id,Year,YearDisplay,YearDisplayAr,Description,DescriptionAr,IsActive,CreatedDate")] VehicleYear vehicleYear)
        {
            if (id != vehicleYear.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleYear.ModifiedDate = DateTime.UtcNow;
                    _context.Update(vehicleYear);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle Year updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleYearExists(vehicleYear.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(VehicleYears));
            }
            return View(vehicleYear);
        }

        // GET: Vehicle/DeleteVehicleYear/5
        public async Task<IActionResult> DeleteVehicleYear(int? id)
        {
            if (id == null) return NotFound();

            var vehicleYear = await _context.VehicleYears
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (vehicleYear == null) return NotFound();

            return View(vehicleYear);
        }

        // POST: Vehicle/DeleteVehicleYearConfirmed/5
        [HttpPost, ActionName("DeleteVehicleYear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleYearConfirmed(int id)
        {
            var vehicleYear = await _context.VehicleYears.FindAsync(id);
            if (vehicleYear != null)
            {
                _context.VehicleYears.Remove(vehicleYear);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Year deleted successfully!";
            }
            return RedirectToAction(nameof(VehicleYears));
        }

        private bool VehicleYearExists(int id)
        {
            return _context.VehicleYears.Any(e => e.Id == id);
        }

        // ============================================
        // ENGINE CAPACITY CRUD OPERATIONS
        // ============================================

        // GET: Vehicle/EngineCapacities
        public async Task<IActionResult> EngineCapacities()
        {
            var capacities = await _context.EngineCapacities
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
            return View(capacities);
        }

        // GET: Vehicle/CreateEngineCapacity
        public IActionResult CreateEngineCapacity()
        {
            return View();
        }

        // POST: Vehicle/CreateEngineCapacity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEngineCapacity([Bind("Capacity,DisplayName,DisplayNameAr,Description,DescriptionAr,IsActive")] EngineCapacity engineCapacity)
        {
            if (ModelState.IsValid)
            {
                // Auto-generate display names if not provided
                if (string.IsNullOrEmpty(engineCapacity.DisplayName))
                    engineCapacity.DisplayName = $"{engineCapacity.Capacity} CC";
                
                if (string.IsNullOrEmpty(engineCapacity.DisplayNameAr))
                    engineCapacity.DisplayNameAr = $"{engineCapacity.Capacity} سي سي";

                engineCapacity.CreatedDate = DateTime.UtcNow;
                _context.Add(engineCapacity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Engine Capacity created successfully!";
                return RedirectToAction(nameof(EngineCapacities));
            }
            return View(engineCapacity);
        }

        // GET: Vehicle/EditEngineCapacity/5
        public async Task<IActionResult> EditEngineCapacity(int? id)
        {
            if (id == null) return NotFound();

            var engineCapacity = await _context.EngineCapacities.FindAsync(id);
            if (engineCapacity == null) return NotFound();

            return View(engineCapacity);
        }

        // POST: Vehicle/EditEngineCapacity/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEngineCapacity(int id, [Bind("Id,Capacity,DisplayName,DisplayNameAr,Description,DescriptionAr,IsActive,CreatedDate")] EngineCapacity engineCapacity)
        {
            if (id != engineCapacity.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    engineCapacity.ModifiedDate = DateTime.UtcNow;
                    _context.Update(engineCapacity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Engine Capacity updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EngineCapacityExists(engineCapacity.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(EngineCapacities));
            }
            return View(engineCapacity);
        }

        // GET: Vehicle/DeleteEngineCapacity/5
        public async Task<IActionResult> DeleteEngineCapacity(int? id)
        {
            if (id == null) return NotFound();

            var engineCapacity = await _context.EngineCapacities
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (engineCapacity == null) return NotFound();

            return View(engineCapacity);
        }

        // POST: Vehicle/DeleteEngineCapacityConfirmed/5
        [HttpPost, ActionName("DeleteEngineCapacity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEngineCapacityConfirmed(int id)
        {
            var engineCapacity = await _context.EngineCapacities.FindAsync(id);
            if (engineCapacity != null)
            {
                _context.EngineCapacities.Remove(engineCapacity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Engine Capacity deleted successfully!";
            }
            return RedirectToAction(nameof(EngineCapacities));
        }

        private bool EngineCapacityExists(int id)
        {
            return _context.EngineCapacities.Any(e => e.Id == id);
        }

        // API: Get Models by Make
        [HttpGet]
        public async Task<JsonResult> GetModelsByMake(int makeId)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var models = await _context.VehicleModels
                .Where(m => m.VehicleMakeId == makeId && m.IsActive)
                .Select(m => new
                {
                    id = m.Id,
                    modelName = m.ModelName,
                    modelNameAr = m.ModelNameAr
                })
                .ToListAsync();
            
            return Json(models);
        }
    }
}