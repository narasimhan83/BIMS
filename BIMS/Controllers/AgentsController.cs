using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class AgentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(ApplicationDbContext context, ILogger<AgentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // AGENT CRUD OPERATIONS
        // ============================================

        // GET: Agents
        public async Task<IActionResult> Index()
        {
            var agents = await _context.Agents
                .Include(a => a.Country)
                .Include(a => a.State)
                .Include(a => a.City)
                .Include(a => a.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
            return View(agents);
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .Include(a => a.Country)
                .Include(a => a.State)
                .Include(a => a.City)
                .Include(a => a.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // GET: Agents/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownLists();
            return View();
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Phone,Mobile,Email,Address,CountryId,StateId,CityId,IsActive")] Agent agent)
        {
            // Try to get bank details from form manually since model binding might not work for dynamic forms
            var bankDetails = new List<AgentBankDetail>();
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

                var bankDetail = new AgentBankDetail
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
                agent.CreatedDate = DateTime.UtcNow;

                _context.Add(agent);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Agent saved with ID: {AgentId}", agent.Id);

                // Add bank details if provided (after agent is saved and has an ID)
                if (bankDetails.Any())
                {
                    foreach (var bankDetail in bankDetails)
                    {
                        bankDetail.AgentId = agent.Id; // Now agent.Id is available
                        _context.AgentBankDetails.Add(bankDetail);
                        _logger.LogInformation("Adding bank detail for agent {AgentId}: {BankId}", agent.Id, bankDetail.BankId);
                    }
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Bank details saved successfully");
                }
                else
                {
                    _logger.LogWarning("No bank details to save");
                }

                TempData["SuccessMessage"] = "Agent created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownLists(agent.CountryId, agent.StateId);
            return View(agent);
        }

        // GET: Agents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .Include(a => a.BankDetails)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
            {
                return NotFound();
            }

            await PopulateDropdownLists(agent.CountryId, agent.StateId);
            return View(agent);
        }

        // POST: Agents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Mobile,Email,Address,CountryId,StateId,CityId,IsActive,CreatedDate")] Agent agent)
        {
            if (id != agent.Id)
            {
                return NotFound();
            }

            // Parse bank details from form manually
            var bankDetails = new List<AgentBankDetail>();
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

                var bankDetail = new AgentBankDetail
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
                    agent.ModifiedDate = DateTime.UtcNow;

                    // Handle bank details
                    var existingBankDetails = await _context.AgentBankDetails
                        .Where(bd => bd.AgentId == id)
                        .ToListAsync();

                    _logger.LogInformation("Edit - Found {Count} existing bank details to remove", existingBankDetails.Count);

                    // Remove existing bank details
                    _context.AgentBankDetails.RemoveRange(existingBankDetails);

                    // Add new bank details
                    if (bankDetails.Any())
                    {
                        foreach (var bankDetail in bankDetails)
                        {
                            bankDetail.AgentId = id;
                            _context.AgentBankDetails.Add(bankDetail);
                            _logger.LogInformation("Edit - Adding bank detail for agent {AgentId}: {BankId}", id, bankDetail.BankId);
                        }
                    }

                    _context.Update(agent);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Edit - Agent and bank details updated successfully");
                    TempData["SuccessMessage"] = "Agent updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgentExists(agent.Id))
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

            await PopulateDropdownLists(agent.CountryId, agent.StateId);
            return View(agent);
        }

        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .Include(a => a.Country)
                .Include(a => a.State)
                .Include(a => a.City)
                .Include(a => a.BankDetails)
                .ThenInclude(bd => bd.Bank)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent != null)
            {
                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Agent deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AgentExists(int id)
        {
            return _context.Agents.Any(e => e.Id == id);
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
    }
}