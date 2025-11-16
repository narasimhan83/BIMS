using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace BIMS.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // GROUPS CRUD OPERATIONS
        // ============================================

        // GET: Customer/Groups
        public async Task<IActionResult> Groups()
        {
            var groups = await _context.Groups
                .OrderByDescending(g => g.CreatedDate)
                .ToListAsync();
            return View(groups);
        }

        // GET: Customer/CreateGroup
        public IActionResult CreateGroup()
        {
            return View();
        }

        // POST: Customer/CreateGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup([Bind("GroupName,GroupNameAr,Description,DescriptionAr,IsActive")] Group group)
        {
            if (ModelState.IsValid)
            {
                group.CreatedDate = DateTime.UtcNow;
                group.CreatedBy = User.Identity?.Name;
                _context.Add(group);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Group created successfully!";
                return RedirectToAction(nameof(Groups));
            }
            return View(group);
        }

        // GET: Customer/EditGroup/5
        public async Task<IActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Customer/EditGroup/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, [Bind("Id,GroupName,GroupNameAr,Description,DescriptionAr,IsActive,CreatedDate,CreatedBy")] Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    group.ModifiedDate = DateTime.UtcNow;
                    group.ModifiedBy = User.Identity?.Name;
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Group updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Groups));
            }
            return View(group);
        }

        // GET: Customer/DeleteGroup/5
        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Customer/DeleteGroupConfirmed/5
        [HttpPost, ActionName("DeleteGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroupConfirmed(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Group deleted successfully!";
            }
            return RedirectToAction(nameof(Groups));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }

        // ============================================
        // CUSTOMERS CRUD OPERATIONS
        // ============================================

        // GET: Customer/Customers
        public async Task<IActionResult> Customers()
        {
            var customers = await _context.Customers
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerGroup)
                .Include(c => c.Agent)
                .Include(c => c.Documents)
                .Include(c => c.Vehicles)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(customers);
        }

        // GET: Customer/CreateCustomer
        public IActionResult CreateCustomer()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Customer/CreateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedDate = DateTime.UtcNow;
                customer.CreatedBy = User.Identity?.Name;
                
                // Process document uploads
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                customer.Documents = new List<CustomerDocument>();
                
                // Find all document file inputs by pattern DocumentFile_X
                int docIndex = 0;
                while (Request.Form.Files[$"DocumentFile_{docIndex}"] != null)
                {
                    var file = Request.Form.Files[$"DocumentFile_{docIndex}"];
                    var documentTypeIdStr = Request.Form[$"DocumentType_{docIndex}"];
                    
                    if (file != null && file.Length > 0 && !string.IsNullOrEmpty(documentTypeIdStr) && int.TryParse(documentTypeIdStr, out int documentTypeId))
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        customer.Documents.Add(new CustomerDocument
                        {
                            DocumentTypeId = documentTypeId,
                            FileName = file.FileName,
                            FilePath = filePath,
                            FileSize = file.Length,
                            ContentType = file.ContentType,
                            UploadedDate = DateTime.UtcNow,
                            UploadedBy = User.Identity?.Name
                        });
                    }
                    docIndex++;
                }

                // Process vehicle information
                var vehicleMakesStr = Request.Form["Vehicles[0].VehicleMakeId"].ToString();
                if (!string.IsNullOrEmpty(vehicleMakesStr))
                {
                    customer.Vehicles = new List<CustomerVehicle>();
                    int vehicleIndex = 0;
                    
                    while (Request.Form.ContainsKey($"Vehicles[{vehicleIndex}].VehicleMakeId"))
                    {
                        var vehicleMakeIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleMakeId"];
                        var vehicleModelIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleModelId"];
                        var vehicleYearIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleYearId"];
                        var engineCapacityIdStr = Request.Form[$"Vehicles[{vehicleIndex}].EngineCapacityId"];
                        var registrationNumber = Request.Form[$"Vehicles[{vehicleIndex}].RegistrationNumber"];
                        var chassisNumber = Request.Form[$"Vehicles[{vehicleIndex}].ChassisNumber"];

                        if (!string.IsNullOrEmpty(vehicleMakeIdStr))
                        {
                            customer.Vehicles.Add(new CustomerVehicle
                            {
                                VehicleMakeId = int.TryParse(vehicleMakeIdStr, out int makeId) ? makeId : null,
                                VehicleModelId = int.TryParse(vehicleModelIdStr, out int modelId) ? modelId : null,
                                VehicleYearId = int.TryParse(vehicleYearIdStr, out int yearId) ? yearId : null,
                                EngineCapacityId = int.TryParse(engineCapacityIdStr, out int capacityId) ? capacityId : null,
                                RegistrationNumber = registrationNumber,
                                ChassisNumber = chassisNumber,
                                CreatedDate = DateTime.UtcNow,
                                IsActive = true
                            });
                        }
                        
                        vehicleIndex++;
                    }
                }

                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer created successfully!";
                return RedirectToAction(nameof(Customers));
            }
            PopulateDropdowns();
            return View(customer);
        }

        // GET: Customer/EditCustomer/5
        public async Task<IActionResult> EditCustomer(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.Documents).ThenInclude(d => d.DocumentType)
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (customer == null) return NotFound();

            PopulateDropdowns(customer.CustomerTypeId, customer.CustomerGroupId);
            return View(customer);
        }

        // POST: Customer/EditCustomer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, Customer customer)
        {
            if (id != customer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers
                        .Include(c => c.Documents)
                        .Include(c => c.Vehicles)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (existingCustomer == null) return NotFound();

                    // Update customer basic info
                    existingCustomer.CustomerTypeId = customer.CustomerTypeId;
                    existingCustomer.CustomerGroupId = customer.CustomerGroupId;
                    existingCustomer.AgentId = customer.AgentId;
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.CPRExpiryDate = customer.CPRExpiryDate;
                    existingCustomer.PassportNo = customer.PassportNo;
                    existingCustomer.PassportIssueDate = customer.PassportIssueDate;
                    existingCustomer.PassportExpiryDate = customer.PassportExpiryDate;
                    existingCustomer.CustomerNameAr = customer.CustomerNameAr;
                    existingCustomer.InsuredName = customer.InsuredName;
                    existingCustomer.InsuredNameAr = customer.InsuredNameAr;
                    existingCustomer.MobilePhone = customer.MobilePhone;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.AddressAr = customer.AddressAr;
                    existingCustomer.CPR_CR_Number = customer.CPR_CR_Number;
                    existingCustomer.VATNumber = customer.VATNumber;
                    existingCustomer.DateOfBirth = customer.DateOfBirth;
                    existingCustomer.IsActive = customer.IsActive;
                    existingCustomer.ModifiedDate = DateTime.UtcNow;
                    existingCustomer.ModifiedBy = User.Identity?.Name;

                    // Process new document uploads
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    // Find all document file inputs by pattern DocumentFile_X
                    int docIndex = 0;
                    while (Request.Form.Files[$"DocumentFile_{docIndex}"] != null)
                    {
                        var file = Request.Form.Files[$"DocumentFile_{docIndex}"];
                        var documentTypeIdStr = Request.Form[$"DocumentType_{docIndex}"];
                        
                        if (file != null && file.Length > 0 && !string.IsNullOrEmpty(documentTypeIdStr) && int.TryParse(documentTypeIdStr, out int documentTypeId))
                        {
                            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var filePath = Path.Combine(uploadsPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            existingCustomer.Documents.Add(new CustomerDocument
                            {
                                CustomerId = existingCustomer.Id,
                                DocumentTypeId = documentTypeId,
                                FileName = file.FileName,
                                FilePath = filePath,
                                FileSize = file.Length,
                                ContentType = file.ContentType,
                                UploadedDate = DateTime.UtcNow,
                                UploadedBy = User.Identity?.Name
                            });
                        }
                        docIndex++;
                    }

                    // Process new vehicle information
                    var vehicleMakesStr = Request.Form["Vehicles[0].VehicleMakeId"].ToString();
                    if (!string.IsNullOrEmpty(vehicleMakesStr))
                    {
                        int vehicleIndex = 0;
                        
                        while (Request.Form.ContainsKey($"Vehicles[{vehicleIndex}].VehicleMakeId"))
                        {
                            var vehicleMakeIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleMakeId"];
                            var vehicleModelIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleModelId"];
                            var vehicleYearIdStr = Request.Form[$"Vehicles[{vehicleIndex}].VehicleYearId"];
                            var engineCapacityIdStr = Request.Form[$"Vehicles[{vehicleIndex}].EngineCapacityId"];
                            var registrationNumber = Request.Form[$"Vehicles[{vehicleIndex}].RegistrationNumber"];
                            var chassisNumber = Request.Form[$"Vehicles[{vehicleIndex}].ChassisNumber"];

                            if (!string.IsNullOrEmpty(vehicleMakeIdStr))
                            {
                                existingCustomer.Vehicles.Add(new CustomerVehicle
                                {
                                    CustomerId = existingCustomer.Id,
                                    VehicleMakeId = int.TryParse(vehicleMakeIdStr, out int makeId) ? makeId : null,
                                    VehicleModelId = int.TryParse(vehicleModelIdStr, out int modelId) ? modelId : null,
                                    VehicleYearId = int.TryParse(vehicleYearIdStr, out int yearId) ? yearId : null,
                                    EngineCapacityId = int.TryParse(engineCapacityIdStr, out int capacityId) ? capacityId : null,
                                    RegistrationNumber = registrationNumber,
                                    ChassisNumber = chassisNumber,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true
                                });
                            }
                            
                            vehicleIndex++;
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Customer updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Customers));
            }
            PopulateDropdowns(customer.CustomerTypeId, customer.CustomerGroupId, customer.AgentId);
            return View(customer);
        }

        // GET: Customer/DetailsCustomer/5
        public async Task<IActionResult> DetailsCustomer(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerGroup)
                .Include(c => c.Agent)
                .Include(c => c.Documents).ThenInclude(d => d.DocumentType)
                .Include(c => c.Vehicles).ThenInclude(v => v.VehicleMake)
                .Include(c => c.Vehicles).ThenInclude(v => v.VehicleModel)
                .Include(c => c.Vehicles).ThenInclude(v => v.VehicleYear)
                .Include(c => c.Vehicles).ThenInclude(v => v.EngineCapacity)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: Customer/DeleteCustomer/5
        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerGroup)
                .Include(c => c.Documents)
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customer/DeleteCustomerConfirmed/5
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Documents)
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (customer != null)
            {
                // Delete associated documents from file system
                if (customer.Documents != null)
                {
                    foreach (var doc in customer.Documents)
                    {
                        if (System.IO.File.Exists(doc.FilePath))
                        {
                            System.IO.File.Delete(doc.FilePath);
                        }
                    }
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer deleted successfully!";
            }
            return RedirectToAction(nameof(Customers));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(object? selectedType = null, object? selectedGroup = null, object? selectedAgent = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Customer Types
            var types = _context.CustomerTypes
                .Where(t => t.IsActive)
                .Select(t => new
                {
                    t.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(t.NameAr)
                        ? t.NameAr : t.Name
                })
                .ToList();
            ViewBag.CustomerTypes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(types, "Id", "DisplayName", selectedType);

            // Groups
            var groups = _context.Groups
                .Where(g => g.IsActive)
                .Select(g => new
                {
                    g.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(g.GroupNameAr)
                        ? g.GroupNameAr : g.GroupName
                })
                .ToList();
            ViewBag.CustomerGroups = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(groups, "Id", "DisplayName", selectedGroup);

            // Agents
            var agents = _context.Agents
                .Where(a => a.IsActive)
                .Select(a => new
                {
                    a.Id,
                    DisplayName = a.Name
                })
                .ToList();
            ViewBag.Agents = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(agents, "Id", "DisplayName", selectedAgent);
            
            // Document Types
            var docTypes = _context.DocumentTypes
                .Where(d => d.IsActive)
                .Select(d => new
                {
                    d.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(d.NameAr)
                        ? d.NameAr : d.Name
                })
                .ToList();
            ViewBag.DocumentTypes = docTypes;
            
            // Vehicle Makes
            var makes = _context.VehicleMakes
                .Where(m => m.IsActive)
                .Select(m => new
                {
                    m.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(m.MakeNameAr)
                        ? m.MakeNameAr : m.MakeName
                })
                .ToList();
            ViewBag.VehicleMakes = makes;
            
            // Vehicle Years
            var years = _context.VehicleYears
                .Where(y => y.IsActive)
                .Select(y => new
                {
                    y.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(y.YearDisplayAr)
                        ? y.YearDisplayAr : (y.YearDisplay ?? y.Year.ToString())
                })
                .ToList();
            ViewBag.VehicleYears = years;
            
            // Engine Capacities
            var capacities = _context.EngineCapacities
                .Where(c => c.IsActive)
                .Select(c => new
                {
                    c.Id,
                    DisplayName = currentLang == "ar" && !string.IsNullOrEmpty(c.DisplayNameAr)
                        ? c.DisplayNameAr : (c.DisplayName ?? $"{c.Capacity} CC")
                })
                .ToList();
            ViewBag.EngineCapacities = capacities;
        }

        // ============================================
        // API ENDPOINTS
        // ============================================

        // API: Get vehicle models by make
        [HttpGet]
        public async Task<IActionResult> GetModelsByMake(int makeId)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            var models = await _context.VehicleModels
                .Where(m => m.VehicleMakeId == makeId && m.IsActive)
                .Select(m => new
                {
                    Id = m.Id,
                    ModelName = currentLang == "ar" && !string.IsNullOrEmpty(m.ModelNameAr)
                        ? m.ModelNameAr
                        : m.ModelName
                })
                .ToListAsync();
            
            return Json(models);
        }

        // POST: Delete Vehicle
        [HttpPost]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.CustomerVehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.CustomerVehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Vehicle deleted successfully" });
            }
            return Json(new { success = false, message = "Vehicle not found" });
        }

        // POST: Delete Document
        [HttpPost]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.CustomerDocuments.FindAsync(id);
            if (document != null)
            {
                // Delete physical file
                if (System.IO.File.Exists(document.FilePath))
                {
                    System.IO.File.Delete(document.FilePath);
                }
                
                _context.CustomerDocuments.Remove(document);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Document deleted successfully" });
            }
            return Json(new { success = false, message = "Document not found" });
        }

        // GET: Download Document
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var document = await _context.CustomerDocuments.FindAsync(id);
            if (document == null || !System.IO.File.Exists(document.FilePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(document.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, document.ContentType ?? "application/octet-stream", document.FileName);
        }

        // ============================================
        // VEHICLE IMPORT OPERATIONS
        // ============================================

        // GET: Download Vehicle Import Template
        [HttpGet]
        public IActionResult DownloadVehicleImportTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Vehicle Import");
                var currentLang = HttpContext.Session.GetString("Language") ?? "en";

                // Define headers
                var headers = currentLang == "ar" 
                    ? new[] { "الصانع", "الموديل", "السنة", "السعة", "رقم التسجيل", "رقم الشاسيه" }
                    : new[] { "Make", "Model", "Year", "Engine Capacity", "Registration Number", "Chassis Number" };

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                    worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Add sample data row
                worksheet.Cells[2, 1].Value = "Toyota";
                worksheet.Cells[2, 2].Value = "Camry";
                worksheet.Cells[2, 3].Value = "2023";
                worksheet.Cells[2, 4].Value = "2.5L";
                worksheet.Cells[2, 5].Value = "ABC-1234";
                worksheet.Cells[2, 6].Value = "JT123456789012345";

                // Add instructions
                worksheet.Cells[4, 1].Value = currentLang == "ar" 
                    ? "ملاحظة: الأعمدة المطلوبة: الصانع، الموديل، السنة. الباقي اختياري." 
                    : "Note: Required columns: Make, Model, Year. Others are optional.";
                worksheet.Cells[4, 1].Style.Font.Italic = true;
                worksheet.Cells[4, 1].Style.Font.Color.SetColor(Color.Gray);

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set column widths
                worksheet.Column(1).Width = 20; // Make
                worksheet.Column(2).Width = 20; // Model
                worksheet.Column(3).Width = 15; // Year
                worksheet.Column(4).Width = 20; // Engine Capacity
                worksheet.Column(5).Width = 25; // Registration
                worksheet.Column(6).Width = 30; // Chassis

                var stream = new MemoryStream(package.GetAsByteArray());
                var fileName = $"Vehicle_Import_Template_{DateTime.Now:yyyyMMdd}.xlsx";
                
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        // POST: Import Vehicles from Excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportVehiclesFromExcel(int customerId, IFormFile excelFile)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";
            
            if (excelFile == null || excelFile.Length == 0)
            {
                return Json(new { success = false, message = currentLang == "ar" ? "الرجاء اختيار ملف" : "Please select a file" });
            }

            // Validate file extension
            var fileExtension = Path.GetExtension(excelFile.FileName).ToLowerInvariant();
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                return Json(new { success = false, message = currentLang == "ar" ? "نوع الملف غير صحيح. يجب أن يكون Excel" : "Invalid file type. Must be Excel file" });
            }

            // Validate file size (5MB)
            if (excelFile.Length > 5 * 1024 * 1024)
            {
                return Json(new { success = false, message = currentLang == "ar" ? "حجم الملف كبير جداً (الحد الأقصى 5MB)" : "File size too large (max 5MB)" });
            }

            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                return Json(new { success = false, message = currentLang == "ar" ? "العميل غير موجود" : "Customer not found" });
            }

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var result = new VehicleImportResult();
                
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension?.Rows ?? 0;

                        if (rowCount < 2)
                        {
                            return Json(new { success = false, message = currentLang == "ar" ? "الملف فارغ" : "File is empty" });
                        }

                        // Load reference data for lookup (cache for performance)
                        var makes = await _context.VehicleMakes
                            .Where(m => m.IsActive)
                            .ToDictionaryAsync(m => m.MakeName.ToLower(), m => m);
                        
                        var makesByArabic = await _context.VehicleMakes
                            .Where(m => m.IsActive && !string.IsNullOrEmpty(m.MakeNameAr))
                            .ToDictionaryAsync(m => m.MakeNameAr!.ToLower(), m => m);
                        
                        var models = await _context.VehicleModels
                            .Where(m => m.IsActive)
                            .ToListAsync();
                        
                        var years = await _context.VehicleYears
                            .Where(y => y.IsActive)
                            .ToDictionaryAsync(y => y.Year, y => y);
                        
                        var capacities = await _context.EngineCapacities
                            .Where(c => c.IsActive)
                            .ToListAsync();

                        var existingRegNumbers = customer.Vehicles?
                            .Where(v => !string.IsNullOrEmpty(v.RegistrationNumber))
                            .Select(v => v.RegistrationNumber!.ToLower())
                            .ToHashSet() ?? new HashSet<string>();

                        var existingChassisNumbers = customer.Vehicles?
                            .Where(v => !string.IsNullOrEmpty(v.ChassisNumber))
                            .Select(v => v.ChassisNumber!.ToLower())
                            .ToHashSet() ?? new HashSet<string>();

                        // Process each row (skip header)
                        for (int row = 2; row <= rowCount; row++)
                        {
                            result.TotalRows++;
                            
                            var makeValue = worksheet.Cells[row, 1].Text.Trim();
                            var modelValue = worksheet.Cells[row, 2].Text.Trim();
                            var yearValue = worksheet.Cells[row, 3].Text.Trim();
                            var capacityValue = worksheet.Cells[row, 4].Text.Trim();
                            var regNumber = worksheet.Cells[row, 5].Text.Trim();
                            var chassisNumber = worksheet.Cells[row, 6].Text.Trim();

                            // Skip empty rows
                            if (string.IsNullOrEmpty(makeValue) && string.IsNullOrEmpty(modelValue))
                                continue;

                            var error = new VehicleImportError
                            {
                                RowNumber = row,
                                Make = makeValue,
                                Model = modelValue,
                                Year = yearValue,
                                EngineCapacity = capacityValue,
                                RegistrationNumber = regNumber,
                                ChassisNumber = chassisNumber
                            };

                            // Validate required fields
                            if (string.IsNullOrEmpty(makeValue))
                            {
                                error.ErrorMessage = currentLang == "ar" ? "الصانع مطلوب" : "Make is required";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            if (string.IsNullOrEmpty(modelValue))
                            {
                                error.ErrorMessage = currentLang == "ar" ? "الموديل مطلوب" : "Model is required";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            if (string.IsNullOrEmpty(yearValue))
                            {
                                error.ErrorMessage = currentLang == "ar" ? "السنة مطلوبة" : "Year is required";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            // Lookup Make (case-insensitive, English or Arabic)
                            VehicleMake? make = null;
                            var makeLower = makeValue.ToLower();
                            
                            if (makes.ContainsKey(makeLower))
                                make = makes[makeLower];
                            else if (makesByArabic.ContainsKey(makeLower))
                                make = makesByArabic[makeLower];

                            if (make == null)
                            {
                                error.ErrorMessage = currentLang == "ar" 
                                    ? $"الصانع '{makeValue}' غير موجود في النظام" 
                                    : $"Make '{makeValue}' not found in system";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            // Lookup Model for this Make
                            var modelLower = modelValue.ToLower();
                            var model = models.FirstOrDefault(m => 
                                m.VehicleMakeId == make.Id && 
                                (m.ModelName.ToLower() == modelLower || 
                                 (!string.IsNullOrEmpty(m.ModelNameAr) && m.ModelNameAr.ToLower() == modelLower)));

                            if (model == null)
                            {
                                error.ErrorMessage = currentLang == "ar" 
                                    ? $"الموديل '{modelValue}' غير موجود للصانع '{makeValue}'" 
                                    : $"Model '{modelValue}' not found for Make '{makeValue}'";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            // Lookup Year
                            if (!int.TryParse(yearValue, out int yearInt))
                            {
                                error.ErrorMessage = currentLang == "ar" ? "السنة غير صحيحة" : "Invalid year format";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            if (!years.ContainsKey(yearInt))
                            {
                                error.ErrorMessage = currentLang == "ar" 
                                    ? $"السنة '{yearValue}' غير موجودة في النظام" 
                                    : $"Year '{yearValue}' not found in system";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            var year = years[yearInt];

                            // Lookup Engine Capacity (optional)
                            EngineCapacity? capacity = null;
                            if (!string.IsNullOrEmpty(capacityValue))
                            {
                                var capacityLowerTrim = capacityValue.ToLower().Trim();
                                capacity = capacities.FirstOrDefault(c =>
                                    c.Capacity.ToString() == capacityValue ||
                                    (c.DisplayName != null && c.DisplayName.ToLower().Contains(capacityLowerTrim)) ||
                                    (c.DisplayNameAr != null && c.DisplayNameAr.ToLower().Contains(capacityLowerTrim)));
                            }

                            // Validate Registration Number uniqueness
                            if (!string.IsNullOrEmpty(regNumber) && existingRegNumbers.Contains(regNumber.ToLower()))
                            {
                                error.ErrorMessage = currentLang == "ar" 
                                    ? $"رقم التسجيل '{regNumber}' موجود مسبقاً" 
                                    : $"Registration number '{regNumber}' already exists";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            // Validate Chassis Number uniqueness
                            if (!string.IsNullOrEmpty(chassisNumber) && existingChassisNumbers.Contains(chassisNumber.ToLower()))
                            {
                                error.ErrorMessage = currentLang == "ar" 
                                    ? $"رقم الشاسيه '{chassisNumber}' موجود مسبقاً" 
                                    : $"Chassis number '{chassisNumber}' already exists";
                                result.Errors.Add(error);
                                result.ErrorCount++;
                                continue;
                            }

                            // Create vehicle
                            var vehicle = new CustomerVehicle
                            {
                                CustomerId = customerId,
                                VehicleMakeId = make.Id,
                                VehicleModelId = model.Id,
                                VehicleYearId = year.Id,
                                EngineCapacityId = capacity?.Id,
                                RegistrationNumber = regNumber,
                                ChassisNumber = chassisNumber,
                                CreatedDate = DateTime.UtcNow,
                                IsActive = true
                            };

                            _context.CustomerVehicles.Add(vehicle);
                            result.ImportedVehicles.Add(vehicle);
                            result.SuccessCount++;

                            // Add to existing lists to prevent duplicates in same import
                            if (!string.IsNullOrEmpty(regNumber))
                                existingRegNumbers.Add(regNumber.ToLower());
                            if (!string.IsNullOrEmpty(chassisNumber))
                                existingChassisNumbers.Add(chassisNumber.ToLower());
                        }

                        // Save all imported vehicles
                        if (result.SuccessCount > 0)
                        {
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    totalRows = result.TotalRows,
                    successCount = result.SuccessCount,
                    errorCount = result.ErrorCount,
                    errors = result.Errors,
                    message = currentLang == "ar" 
                        ? $"تم استيراد {result.SuccessCount} من {result.TotalRows} مركبة" 
                        : $"Imported {result.SuccessCount} of {result.TotalRows} vehicles"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing vehicles");
                return Json(new
                {
                    success = false,
                    message = currentLang == "ar"
                        ? $"خطأ في الاستيراد: {ex.Message}"
                        : $"Import error: {ex.Message}"
                });
            }
        }
    }
}