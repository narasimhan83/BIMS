using BIMS.Data;
using BIMS.Models;
using BIMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class InsuranceClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InsuranceClientsController> _logger;

        public InsuranceClientsController(ApplicationDbContext context, ILogger<InsuranceClientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // INSURANCE CLIENT CRUD OPERATIONS
        // ============================================

        // GET: InsuranceClients
        public async Task<IActionResult> Index()
        {
            var insuranceClients = await _context.InsuranceClients
                .Include(ic => ic.Country)
                .Include(ic => ic.State)
                .Include(ic => ic.City)
                .Include(ic => ic.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .OrderByDescending(ic => ic.CreatedDate)
                .ToListAsync();
            return View(insuranceClients);
        }

        // GET: InsuranceClients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceClient = await _context.InsuranceClients
                .Include(ic => ic.Country)
                .Include(ic => ic.State)
                .Include(ic => ic.City)
                .Include(ic => ic.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (insuranceClient == null)
            {
                return NotFound();
            }

            return View(insuranceClient);
        }

        // GET: InsuranceClients/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownLists();
            return View();
        }

        // POST: InsuranceClients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,NameAr,Phone,Mobile,Email,Address,CountryId,StateId,CityId,IsActive")] InsuranceClient insuranceClient)
        {
            // Try to get bank details from form manually since model binding might not work for dynamic forms
            var bankDetails = new List<InsuranceClientBankDetail>();
            var form = HttpContext.Request.Form;

            // Debug: Log all form keys
            _logger.LogInformation("Form keys: {Keys}", string.Join(", ", form.Keys));

            // Parse bank details from form data
            for (int i = 0; ; i++)
            {
                var bankIdKey = $"bankDetails[{i}].BankId";
                var ibanKey = $"bankDetails[{i}].IbanNumber";
                var swiftKey = $"bankDetails[{i}].SwiftCode";
                var branchKey = $"bankDetails[{i}].Branch";
                var isPrimaryKey = $"bankDetails[{i}].IsPrimary";
                var notesKey = $"bankDetails[{i}].Notes";

                if (!form.ContainsKey(bankIdKey) || string.IsNullOrEmpty(form[bankIdKey]))
                {
                    _logger.LogInformation("No more bank details at index {Index}, key {Key}", i, bankIdKey);
                    break; // No more bank details
                }

                var bankDetail = new InsuranceClientBankDetail
                {
                    BankId = int.TryParse(form[bankIdKey], out var bankId) ? bankId : 0,
                    IbanNumber = form[ibanKey].ToString(),
                    SwiftCode = form[swiftKey].ToString(),
                    Branch = form[branchKey].ToString(),
                    IsPrimary = form[isPrimaryKey].ToString().ToLower() == "true" || form[isPrimaryKey].ToString() == "on",
                    Notes = form[notesKey].ToString(),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                _logger.LogInformation("Parsed bank detail {Index}: BankId={BankId}, IBAN={Iban}, Swift={Swift}, Branch={Branch}",
                    i, bankDetail.BankId, bankDetail.IbanNumber, bankDetail.SwiftCode, bankDetail.Branch);

                // Only add if required fields are filled
                if (!string.IsNullOrEmpty(bankDetail.IbanNumber) &&
                    !string.IsNullOrEmpty(bankDetail.SwiftCode) &&
                    !string.IsNullOrEmpty(bankDetail.Branch) &&
                    bankDetail.BankId > 0)
                {
                    bankDetails.Add(bankDetail);
                    _logger.LogInformation("Added bank detail {Index} to collection", i);
                }
                else
                {
                    _logger.LogWarning("Bank detail {Index} not added - missing required fields", i);
                }
            }

            _logger.LogInformation("Total bank details parsed: {Count}", bankDetails.Count);

            if (ModelState.IsValid)
            {
                insuranceClient.CreatedDate = DateTime.UtcNow;

                _context.Add(insuranceClient);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Insurance client saved with ID: {ClientId}", insuranceClient.Id);

                // Add bank details if provided (after client is saved and has an ID)
                if (bankDetails.Any())
                {
                    foreach (var bankDetail in bankDetails)
                    {
                        bankDetail.InsuranceClientId = insuranceClient.Id; // Now insuranceClient.Id is available
                        _context.InsuranceClientBankDetails.Add(bankDetail);
                        _logger.LogInformation("Adding bank detail for client {ClientId}: {BankId}", insuranceClient.Id, bankDetail.BankId);
                    }
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Bank details saved successfully");
                }
                else
                {
                    _logger.LogWarning("No bank details to save");
                }

                TempData["SuccessMessage"] = "Insurance client created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownLists(insuranceClient.CountryId, insuranceClient.StateId);
            return View(insuranceClient);
        }

        // GET: InsuranceClients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceClient = await _context.InsuranceClients
                .Include(ic => ic.BankDetails)
                .FirstOrDefaultAsync(ic => ic.Id == id);

            if (insuranceClient == null)
            {
                return NotFound();
            }

            await PopulateDropdownLists(insuranceClient.CountryId, insuranceClient.StateId);
            return View(insuranceClient);
        }

        // POST: InsuranceClients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NameAr,Phone,Mobile,Email,Address,CountryId,StateId,CityId,IsActive,CreatedDate")] InsuranceClient insuranceClient)
        {
            if (id != insuranceClient.Id)
            {
                return NotFound();
            }

            // Parse bank details from form manually
            var bankDetails = new List<InsuranceClientBankDetail>();
            var form = HttpContext.Request.Form;

            _logger.LogInformation("Edit - Form keys: {Keys}", string.Join(", ", form.Keys));

            for (int i = 0; ; i++)
            {
                var bankIdKey = $"bankDetails[{i}].BankId";
                var ibanKey = $"bankDetails[{i}].IbanNumber";
                var swiftKey = $"bankDetails[{i}].SwiftCode";
                var branchKey = $"bankDetails[{i}].Branch";
                var isPrimaryKey = $"bankDetails[{i}].IsPrimary";
                var notesKey = $"bankDetails[{i}].Notes";

                if (!form.ContainsKey(bankIdKey) || string.IsNullOrEmpty(form[bankIdKey]))
                {
                    _logger.LogInformation("Edit - No more bank details at index {Index}", i);
                    break; // No more bank details
                }

                var bankDetail = new InsuranceClientBankDetail
                {
                    BankId = int.TryParse(form[bankIdKey], out var bankId) ? bankId : 0,
                    IbanNumber = form[ibanKey].ToString(),
                    SwiftCode = form[swiftKey].ToString(),
                    Branch = form[branchKey].ToString(),
                    IsPrimary = form[isPrimaryKey].ToString().ToLower() == "true" || form[isPrimaryKey].ToString() == "on",
                    Notes = form[notesKey].ToString(),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                _logger.LogInformation("Edit - Parsed bank detail {Index}: BankId={BankId}, IBAN={Iban}, Swift={Swift}, Branch={Branch}",
                    i, bankDetail.BankId, bankDetail.IbanNumber, bankDetail.SwiftCode, bankDetail.Branch);

                // Only add if required fields are filled
                if (!string.IsNullOrEmpty(bankDetail.IbanNumber) &&
                    !string.IsNullOrEmpty(bankDetail.SwiftCode) &&
                    !string.IsNullOrEmpty(bankDetail.Branch) &&
                    bankDetail.BankId > 0)
                {
                    bankDetails.Add(bankDetail);
                    _logger.LogInformation("Edit - Added bank detail {Index} to collection", i);
                }
                else
                {
                    _logger.LogWarning("Edit - Bank detail {Index} not added - missing required fields", i);
                }
            }

            _logger.LogInformation("Edit - Total bank details parsed: {Count}", bankDetails.Count);

            if (ModelState.IsValid)
            {
                try
                {
                    insuranceClient.ModifiedDate = DateTime.UtcNow;

                    // Handle bank details
                    var existingBankDetails = await _context.InsuranceClientBankDetails
                        .Where(bd => bd.InsuranceClientId == id)
                        .ToListAsync();

                    _logger.LogInformation("Edit - Found {Count} existing bank details to remove", existingBankDetails.Count);

                    // Remove existing bank details
                    _context.InsuranceClientBankDetails.RemoveRange(existingBankDetails);

                    // Add new bank details
                    if (bankDetails.Any())
                    {
                        foreach (var bankDetail in bankDetails)
                        {
                            bankDetail.InsuranceClientId = id;
                            _context.InsuranceClientBankDetails.Add(bankDetail);
                            _logger.LogInformation("Edit - Adding bank detail for client {ClientId}: {BankId}", id, bankDetail.BankId);
                        }
                    }

                    _context.Update(insuranceClient);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Edit - Insurance client and bank details updated successfully");
                    TempData["SuccessMessage"] = "Insurance client updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceClientExists(insuranceClient.Id))
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

            await PopulateDropdownLists(insuranceClient.CountryId, insuranceClient.StateId);
            return View(insuranceClient);
        }

        // GET: InsuranceClients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceClient = await _context.InsuranceClients
                .Include(ic => ic.Country)
                .Include(ic => ic.State)
                .Include(ic => ic.City)
                .Include(ic => ic.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (insuranceClient == null)
            {
                return NotFound();
            }

            return View(insuranceClient);
        }

        // POST: InsuranceClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceClient = await _context.InsuranceClients.FindAsync(id);
            if (insuranceClient != null)
            {
                _context.InsuranceClients.Remove(insuranceClient);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance client deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceClientExists(int id)
        {
            return _context.InsuranceClients.Any(e => e.Id == id);
        }

        // Helper method to populate dropdown lists
        private async Task PopulateDropdownLists(int? selectedCountryId = null, int? selectedStateId = null)
        {
            // Countries
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Countries = new SelectList(countries, "Id", "Name", selectedCountryId);

            // States - filtered by selected country if provided
            var statesQuery = _context.States.Where(s => s.IsActive);
            if (selectedCountryId.HasValue)
            {
                statesQuery = statesQuery.Where(s => s.CountryId == selectedCountryId.Value);
            }
            var states = await statesQuery
                .Include(s => s.Country)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.States = new SelectList(states, "Id", "Name", selectedStateId);

            // Cities - filtered by selected state if provided
            var citiesQuery = _context.Cities.Where(c => c.IsActive);
            if (selectedStateId.HasValue)
            {
                citiesQuery = citiesQuery.Where(c => c.StateId == selectedStateId.Value);
            }
            var cities = await citiesQuery
                .Include(c => c.State)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Cities = new SelectList(cities, "Id", "Name");

            // Banks
            var banks = await _context.Banks
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();
            ViewBag.Banks = new SelectList(banks, "Id", "Name");
        }

        // AJAX method to get states by country
        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry(int countryId)
        {
            var states = await _context.States
                .Where(s => s.CountryId == countryId && s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Json(states);
        }

        // AJAX method to get cities by state
        [HttpGet]
        public async Task<IActionResult> GetCitiesByState(int stateId)
        {
            var cities = await _context.Cities
                .Where(c => c.StateId == stateId && c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            return Json(cities);
        }

        // ============================================
        // LINE OF BUSINESS CRUD OPERATIONS
        // ============================================

        // GET: InsuranceClients/LinesOfBusiness
        public async Task<IActionResult> LinesOfBusiness()
        {
            var linesOfBusiness = await _context.LinesOfBusiness
                .Include(lob => lob.InsuranceClient)
                .OrderByDescending(lob => lob.CreatedDate)
                .ToListAsync();
            return View(linesOfBusiness);
        }

        // GET: InsuranceClients/CreateLineOfBusiness
        public async Task<IActionResult> CreateLineOfBusiness()
        {
            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name");
            return View();
        }

        // POST: InsuranceClients/CreateLineOfBusiness
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLineOfBusiness([Bind("InsuranceClientId,Name,NameAr,Code,Description,DescriptionAr,IsActive")] LineOfBusiness lineOfBusiness)
        {
            if (ModelState.IsValid)
            {
                lineOfBusiness.CreatedDate = DateTime.UtcNow;
                _context.Add(lineOfBusiness);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Line of Business created successfully!";
                return RedirectToAction(nameof(LinesOfBusiness));
            }

            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name", lineOfBusiness.InsuranceClientId);
            return View(lineOfBusiness);
        }

        // GET: InsuranceClients/EditLineOfBusiness/5
        public async Task<IActionResult> EditLineOfBusiness(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineOfBusiness = await _context.LinesOfBusiness.FindAsync(id);
            if (lineOfBusiness == null)
            {
                return NotFound();
            }

            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name", lineOfBusiness.InsuranceClientId);

            return View(lineOfBusiness);
        }

        // POST: InsuranceClients/EditLineOfBusiness/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLineOfBusiness(int id, [Bind("Id,InsuranceClientId,Name,NameAr,Code,Description,DescriptionAr,IsActive,CreatedDate")] LineOfBusiness lineOfBusiness)
        {
            if (id != lineOfBusiness.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lineOfBusiness.ModifiedDate = DateTime.UtcNow;
                    _context.Update(lineOfBusiness);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Line of Business updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineOfBusinessExists(lineOfBusiness.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(LinesOfBusiness));
            }

            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name", lineOfBusiness.InsuranceClientId);

            return View(lineOfBusiness);
        }

        // GET: InsuranceClients/DeleteLineOfBusiness/5
        public async Task<IActionResult> DeleteLineOfBusiness(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineOfBusiness = await _context.LinesOfBusiness
                .Include(lob => lob.InsuranceClient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineOfBusiness == null)
            {
                return NotFound();
            }

            return View(lineOfBusiness);
        }

        // POST: InsuranceClients/DeleteLineOfBusinessConfirmed/5
        [HttpPost, ActionName("DeleteLineOfBusiness")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLineOfBusinessConfirmed(int id)
        {
            var lineOfBusiness = await _context.LinesOfBusiness.FindAsync(id);
            if (lineOfBusiness != null)
            {
                _context.LinesOfBusiness.Remove(lineOfBusiness);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Line of Business deleted successfully!";
            }
            return RedirectToAction(nameof(LinesOfBusiness));
        }

        private bool LineOfBusinessExists(int id)
        {
            return _context.LinesOfBusiness.Any(e => e.Id == id);
        }
    }
}