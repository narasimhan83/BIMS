using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class MastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MastersController> _logger;

        public MastersController(ApplicationDbContext context, ILogger<MastersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // CALCULATION METHOD CRUD OPERATIONS
        // ============================================

        // GET: Masters/CalculationMethods
        public async Task<IActionResult> CalculationMethods()
        {
            var calculationMethods = await _context.CalculationMethods
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(calculationMethods);
        }

        // GET: Masters/CreateCalculationMethod
        public IActionResult CreateCalculationMethod()
        {
            return View();
        }

        // POST: Masters/CreateCalculationMethod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCalculationMethod([Bind("Name,NameAr,Code,Description,DescriptionAr,Formula,FormulaAr,IsActive")] CalculationMethod calculationMethod)
        {
            if (ModelState.IsValid)
            {
                calculationMethod.CreatedDate = DateTime.UtcNow;
                _context.Add(calculationMethod);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Calculation Method created successfully!";
                return RedirectToAction(nameof(CalculationMethods));
            }
            return View(calculationMethod);
        }

        // GET: Masters/EditCalculationMethod/5
        public async Task<IActionResult> EditCalculationMethod(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculationMethod = await _context.CalculationMethods.FindAsync(id);
            if (calculationMethod == null)
            {
                return NotFound();
            }
            return View(calculationMethod);
        }

        // POST: Masters/EditCalculationMethod/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCalculationMethod(int id, [Bind("Id,Name,NameAr,Code,Description,DescriptionAr,Formula,FormulaAr,IsActive,CreatedDate")] CalculationMethod calculationMethod)
        {
            if (id != calculationMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    calculationMethod.ModifiedDate = DateTime.UtcNow;
                    _context.Update(calculationMethod);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Calculation Method updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalculationMethodExists(calculationMethod.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CalculationMethods));
            }
            return View(calculationMethod);
        }

        // GET: Masters/DeleteCalculationMethod/5
        public async Task<IActionResult> DeleteCalculationMethod(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculationMethod = await _context.CalculationMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calculationMethod == null)
            {
                return NotFound();
            }

            return View(calculationMethod);
        }

        // POST: Masters/DeleteCalculationMethodConfirmed/5
        [HttpPost, ActionName("DeleteCalculationMethod")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCalculationMethodConfirmed(int id)
        {
            var calculationMethod = await _context.CalculationMethods.FindAsync(id);
            if (calculationMethod != null)
            {
                _context.CalculationMethods.Remove(calculationMethod);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Calculation Method deleted successfully!";
            }
            return RedirectToAction(nameof(CalculationMethods));
        }

        private bool CalculationMethodExists(int id)
        {
            return _context.CalculationMethods.Any(e => e.Id == id);
        }

        // ============================================
        // COUNTRY CRUD OPERATIONS
        // ============================================

        // GET: Masters/Countries
        public async Task<IActionResult> Countries()
        {
            var countries = await _context.Countries
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(countries);
        }

        // GET: Masters/CreateCountry
        public IActionResult CreateCountry()
        {
            return View();
        }

        // POST: Masters/CreateCountry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry([Bind("Name,NameAr,Code,PhoneCode,Description,DescriptionAr,IsActive")] Country country)
        {
            if (ModelState.IsValid)
            {
                country.CreatedDate = DateTime.UtcNow;
                _context.Add(country);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Country created successfully!";
                return RedirectToAction(nameof(Countries));
            }
            return View(country);
        }

        // GET: Masters/EditCountry/5
        public async Task<IActionResult> EditCountry(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Masters/EditCountry/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCountry(int id, [Bind("Id,Name,NameAr,Code,PhoneCode,Description,DescriptionAr,IsActive,CreatedDate")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    country.ModifiedDate = DateTime.UtcNow;
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Country updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Countries));
            }
            return View(country);
        }

        // GET: Masters/DeleteCountry/5
        public async Task<IActionResult> DeleteCountry(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Masters/DeleteCountryConfirmed/5
        [HttpPost, ActionName("DeleteCountry")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCountryConfirmed(int id)
        {
            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (country != null)
            {
                if (country.States?.Any() == true)
                {
                    TempData["ErrorMessage"] = "Cannot delete country with associated states. Please delete all states first.";
                    return RedirectToAction(nameof(Countries));
                }
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Country deleted successfully!";
            }
            return RedirectToAction(nameof(Countries));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }

        // ============================================
        // STATE CRUD OPERATIONS
        // ============================================

        // GET: Masters/States
        public async Task<IActionResult> States()
        {
            var states = await _context.States
                .Include(s => s.Country)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
            return View(states);
        }

        // GET: Masters/CreateState
        public async Task<IActionResult> CreateState()
        {
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Countries = countries;
            return View();
        }

        // POST: Masters/CreateState
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateState([Bind("Name,NameAr,Code,CountryId,Description,DescriptionAr,IsActive")] State state)
        {
            if (ModelState.IsValid)
            {
                state.CreatedDate = DateTime.UtcNow;
                _context.Add(state);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "State created successfully!";
                return RedirectToAction(nameof(States));
            }
            
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Countries = countries;
            return View(state);
        }

        // GET: Masters/EditState/5
        public async Task<IActionResult> EditState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Countries = countries;
            
            return View(state);
        }

        // POST: Masters/EditState/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, [Bind("Id,Name,NameAr,Code,CountryId,Description,DescriptionAr,IsActive,CreatedDate")] State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    state.ModifiedDate = DateTime.UtcNow;
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "State updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(States));
            }
            
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Countries = countries;
            
            return View(state);
        }

        // GET: Masters/DeleteState/5
        public async Task<IActionResult> DeleteState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .Include(s => s.Country)
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: Masters/DeleteStateConfirmed/5
        [HttpPost, ActionName("DeleteState")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStateConfirmed(int id)
        {
            var state = await _context.States
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state != null)
            {
                if (state.Cities?.Any() == true)
                {
                    TempData["ErrorMessage"] = "Cannot delete state with associated cities. Please delete all cities first.";
                    return RedirectToAction(nameof(States));
                }
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "State deleted successfully!";
            }
            return RedirectToAction(nameof(States));
        }

        private bool StateExists(int id)
        {
            return _context.States.Any(e => e.Id == id);
        }

        // ============================================
        // CITY CRUD OPERATIONS
        // ============================================

        // GET: Masters/Cities
        public async Task<IActionResult> Cities()
        {
            var cities = await _context.Cities
                .Include(c => c.State)
                .ThenInclude(s => s.Country)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(cities);
        }

        // GET: Masters/CreateCity
        public async Task<IActionResult> CreateCity()
        {
            var states = await _context.States
                .Include(s => s.Country)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.States = states;
            return View();
        }

        // POST: Masters/CreateCity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCity([Bind("Name,NameAr,Code,StateId,Description,DescriptionAr,IsActive")] City city)
        {
            if (ModelState.IsValid)
            {
                city.CreatedDate = DateTime.UtcNow;
                _context.Add(city);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "City created successfully!";
                return RedirectToAction(nameof(Cities));
            }
            
            var states = await _context.States
                .Include(s => s.Country)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.States = states;
            return View(city);
        }

        // GET: Masters/EditCity/5
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            
            var states = await _context.States
                .Include(s => s.Country)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.States = states;
            
            return View(city);
        }

        // POST: Masters/EditCity/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, [Bind("Id,Name,NameAr,Code,StateId,Description,DescriptionAr,IsActive,CreatedDate")] City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    city.ModifiedDate = DateTime.UtcNow;
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "City updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Cities));
            }
            
            var states = await _context.States
                .Include(s => s.Country)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.States = states;
            
            return View(city);
        }

        // GET: Masters/DeleteCity/5
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.State)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Masters/DeleteCityConfirmed/5
        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "City deleted successfully!";
            }
            return RedirectToAction(nameof(Cities));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }

        // ============================================
        // CUSTOMER TYPE CRUD OPERATIONS
        // ============================================

        // GET: Masters/CustomerTypes
        public async Task<IActionResult> CustomerTypes()
        {
            var customerTypes = await _context.CustomerTypes
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(customerTypes);
        }

        // GET: Masters/CreateCustomerType
        public IActionResult CreateCustomerType()
        {
            return View();
        }

        // POST: Masters/CreateCustomerType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomerType([Bind("Name,Description,IsActive")] CustomerType customerType)
        {
            if (ModelState.IsValid)
            {
                customerType.CreatedDate = DateTime.UtcNow;
                _context.Add(customerType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer Type created successfully!";
                return RedirectToAction(nameof(CustomerTypes));
            }
            return View(customerType);
        }

        // GET: Masters/EditCustomerType/5
        public async Task<IActionResult> EditCustomerType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerType = await _context.CustomerTypes.FindAsync(id);
            if (customerType == null)
            {
                return NotFound();
            }
            return View(customerType);
        }

        // POST: Masters/EditCustomerType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerType(int id, [Bind("Id,Name,Description,IsActive,CreatedDate")] CustomerType customerType)
        {
            if (id != customerType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customerType.ModifiedDate = DateTime.UtcNow;
                    _context.Update(customerType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Customer Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerTypeExists(customerType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerTypes));
            }
            return View(customerType);
        }

        // GET: Masters/DeleteCustomerType/5
        public async Task<IActionResult> DeleteCustomerType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerType = await _context.CustomerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerType == null)
            {
                return NotFound();
            }

            return View(customerType);
        }

        // POST: Masters/DeleteCustomerTypeConfirmed/5
        [HttpPost, ActionName("DeleteCustomerType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerTypeConfirmed(int id)
        {
            var customerType = await _context.CustomerTypes.FindAsync(id);
            if (customerType != null)
            {
                _context.CustomerTypes.Remove(customerType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Customer Type deleted successfully!";
            }
            return RedirectToAction(nameof(CustomerTypes));
        }

        private bool CustomerTypeExists(int id)
        {
            return _context.CustomerTypes.Any(e => e.Id == id);
        }

        // ============================================
        // DOCUMENT TYPE CRUD OPERATIONS
        // ============================================

        // GET: Masters/DocumentTypes
        public async Task<IActionResult> DocumentTypes()
        {
            var documentTypes = await _context.DocumentTypes
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
            return View(documentTypes);
        }

        // GET: Masters/CreateDocumentType
        public IActionResult CreateDocumentType()
        {
            return View();
        }

        // POST: Masters/CreateDocumentType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDocumentType([Bind("Name,Code,Description,IsActive")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                documentType.CreatedDate = DateTime.UtcNow;
                _context.Add(documentType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Document Type created successfully!";
                return RedirectToAction(nameof(DocumentTypes));
            }
            return View(documentType);
        }

        // GET: Masters/EditDocumentType/5
        public async Task<IActionResult> EditDocumentType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return NotFound();
            }
            return View(documentType);
        }

        // POST: Masters/EditDocumentType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDocumentType(int id, [Bind("Id,Name,Code,Description,IsActive,CreatedDate")] DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    documentType.ModifiedDate = DateTime.UtcNow;
                    _context.Update(documentType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Document Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentTypeExists(documentType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DocumentTypes));
            }
            return View(documentType);
        }

        // GET: Masters/DeleteDocumentType/5
        public async Task<IActionResult> DeleteDocumentType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: Masters/DeleteDocumentTypeConfirmed/5
        [HttpPost, ActionName("DeleteDocumentType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDocumentTypeConfirmed(int id)
        {
            var documentType = await _context.DocumentTypes.FindAsync(id);
            if (documentType != null)
            {
                _context.DocumentTypes.Remove(documentType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Document Type deleted successfully!";
            }
            return RedirectToAction(nameof(DocumentTypes));
        }

        private bool DocumentTypeExists(int id)
        {
            return _context.DocumentTypes.Any(e => e.Id == id);
        }

        // ============================================
        // BUSINESS TYPE CRUD OPERATIONS
        // ============================================

        // GET: Masters/BusinessTypes
        public async Task<IActionResult> BusinessTypes()
        {
            var businessTypes = await _context.BusinessTypes
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
            return View(businessTypes);
        }

        // GET: Masters/CreateBusinessType
        public IActionResult CreateBusinessType()
        {
            return View();
        }

        // POST: Masters/CreateBusinessType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBusinessType([Bind("Name,Code,Description,IsActive")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                businessType.CreatedDate = DateTime.UtcNow;
                _context.Add(businessType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Business Type created successfully!";
                return RedirectToAction(nameof(BusinessTypes));
            }
            return View(businessType);
        }

        // GET: Masters/EditBusinessType/5
        public async Task<IActionResult> EditBusinessType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessType = await _context.BusinessTypes.FindAsync(id);
            if (businessType == null)
            {
                return NotFound();
            }
            return View(businessType);
        }

        // POST: Masters/EditBusinessType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBusinessType(int id, [Bind("Id,Name,Code,Description,IsActive,CreatedDate")] BusinessType businessType)
        {
            if (id != businessType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    businessType.ModifiedDate = DateTime.UtcNow;
                    _context.Update(businessType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Business Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessTypeExists(businessType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BusinessTypes));
            }
            return View(businessType);
        }

        // GET: Masters/DeleteBusinessType/5
        public async Task<IActionResult> DeleteBusinessType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessType = await _context.BusinessTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessType == null)
            {
                return NotFound();
            }

            return View(businessType);
        }

        // POST: Masters/DeleteBusinessTypeConfirmed/5
        [HttpPost, ActionName("DeleteBusinessType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBusinessTypeConfirmed(int id)
        {
            var businessType = await _context.BusinessTypes.FindAsync(id);
            if (businessType != null)
            {
                _context.BusinessTypes.Remove(businessType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Business Type deleted successfully!";
            }
            return RedirectToAction(nameof(BusinessTypes));
        }

        private bool BusinessTypeExists(int id)
        {
            return _context.BusinessTypes.Any(e => e.Id == id);
        }

        // ============================================
        // SALES TYPE CRUD OPERATIONS
        // ============================================

        // GET: Masters/SalesTypes
        public async Task<IActionResult> SalesTypes()
        {
            var salesTypes = await _context.SalesTypes
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
            return View(salesTypes);
        }

        // GET: Masters/CreateSalesType
        public IActionResult CreateSalesType()
        {
            return View();
        }

        // POST: Masters/CreateSalesType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSalesType([Bind("Name,NameAr,Code,Description,DescriptionAr,IsActive")] SalesType salesType)
        {
            if (ModelState.IsValid)
            {
                salesType.CreatedDate = DateTime.UtcNow;
                _context.Add(salesType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sales Type created successfully!";
                return RedirectToAction(nameof(SalesTypes));
            }
            return View(salesType);
        }

        // GET: Masters/EditSalesType/5
        public async Task<IActionResult> EditSalesType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesType = await _context.SalesTypes.FindAsync(id);
            if (salesType == null)
            {
                return NotFound();
            }
            return View(salesType);
        }

        // POST: Masters/EditSalesType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSalesType(int id, [Bind("Id,Name,NameAr,Code,Description,DescriptionAr,IsActive,CreatedDate")] SalesType salesType)
        {
            if (id != salesType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    salesType.ModifiedDate = DateTime.UtcNow;
                    _context.Update(salesType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sales Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesTypeExists(salesType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SalesTypes));
            }
            return View(salesType);
        }

        // GET: Masters/DeleteSalesType/5
        public async Task<IActionResult> DeleteSalesType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesType = await _context.SalesTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesType == null)
            {
                return NotFound();
            }

            return View(salesType);
        }

        // POST: Masters/DeleteSalesTypeConfirmed/5
        [HttpPost, ActionName("DeleteSalesType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSalesTypeConfirmed(int id)
        {
            var salesType = await _context.SalesTypes.FindAsync(id);
            if (salesType != null)
            {
                _context.SalesTypes.Remove(salesType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sales Type deleted successfully!";
            }
            return RedirectToAction(nameof(SalesTypes));
        }

        private bool SalesTypeExists(int id)
        {
            return _context.SalesTypes.Any(e => e.Id == id);
        }

        // ============================================
        // VEHICLE TYPE CRUD OPERATIONS
        // ============================================

        // GET: Masters/VehicleTypes
        public async Task<IActionResult> VehicleTypes()
        {
            var vehicleTypes = await _context.VehicleTypes
                .OrderByDescending(v => v.CreatedDate)
                .ToListAsync();
            return View(vehicleTypes);
        }

        // GET: Masters/CreateVehicleType
        public IActionResult CreateVehicleType()
        {
            return View();
        }

        // POST: Masters/CreateVehicleType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicleType([Bind("TypeName,TypeNameAr,Code,Description,DescriptionAr,IsActive")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                vehicleType.CreatedDate = DateTime.UtcNow;
                _context.Add(vehicleType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Type created successfully!";
                return RedirectToAction(nameof(VehicleTypes));
            }
            return View(vehicleType);
        }

        // GET: Masters/EditVehicleType/5
        public async Task<IActionResult> EditVehicleType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        // POST: Masters/EditVehicleType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicleType(int id, [Bind("Id,TypeName,TypeNameAr,Code,Description,DescriptionAr,IsActive,CreatedDate")] VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleType.ModifiedDate = DateTime.UtcNow;
                    _context.Update(vehicleType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle Type updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VehicleTypes));
            }
            return View(vehicleType);
        }

        // GET: Masters/DeleteVehicleType/5
        public async Task<IActionResult> DeleteVehicleType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: Masters/DeleteVehicleTypeConfirmed/5
        [HttpPost, ActionName("DeleteVehicleType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicleTypeConfirmed(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType != null)
            {
                _context.VehicleTypes.Remove(vehicleType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle Type deleted successfully!";
            }
            return RedirectToAction(nameof(VehicleTypes));
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.Id == id);
        }
    }
}