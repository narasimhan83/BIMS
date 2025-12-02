using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BIMS.Controllers
{
    [Authorize]
    public class LeadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeadsController> _logger;

        public LeadsController(ApplicationDbContext context, ILogger<LeadsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Leads
        public async Task<IActionResult> Index()
        {
            var leads = await _context.Leads
                .Include(l => l.Customer)
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();
            return View(leads);
        }

        // GET: Leads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads
                .Include(l => l.Customer)
                .Include(l => l.Proposals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // GET: Leads/Create
        public IActionResult Create()
        {
            PopulateLeadSourceList();
            return View();
        }

        // POST: Leads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameEn,NameAr,Email,Phone,CompanyEn,CompanyAr,Source,Status,PotentialValue,Notes,RequestDate,LastContactDate,CustomerId")] Lead lead)
        {
            // Ensure a customer is selected
            if (lead.CustomerId == 0)
            {
                ModelState.AddModelError(nameof(lead.CustomerId), "Customer is required");
            }
            else
            {
                // Auto-populate core lead fields from the selected customer
                var customer = await _context.Customers
                    .Include(c => c.CustomerGroup)
                    .FirstOrDefaultAsync(c => c.Id == lead.CustomerId && c.IsActive);

                if (customer == null)
                {
                    ModelState.AddModelError(nameof(lead.CustomerId), "Selected customer not found or inactive");
                }
                else
                {
                    // Auto-fill required fields from customer
                    lead.NameEn = customer.CustomerName ?? customer.CustomerNameAr ?? "Customer";
                    lead.NameAr = customer.CustomerNameAr ?? customer.CustomerName ?? "Customer";
                    lead.Email = string.IsNullOrWhiteSpace(customer.Email)
                        ? "no-email@placeholder.local"
                        : customer.Email;
                    lead.Phone = string.IsNullOrWhiteSpace(customer.MobilePhone)
                        ? "00000000"
                        : customer.MobilePhone;
                    lead.CompanyEn = lead.CompanyEn ?? customer.CustomerGroup?.GroupName;
                    lead.CompanyAr = lead.CompanyAr ?? customer.CustomerGroup?.GroupName;

                    // If PotentialValue not provided in UI, default it to 0
                    if (lead.PotentialValue == 0)
                    {
                        lead.PotentialValue = 0m;
                    }

                    // Clear model state entries for fields we just populated so validation reruns
                    ModelState.Remove(nameof(lead.NameEn));
                    ModelState.Remove(nameof(lead.NameAr));
                    ModelState.Remove(nameof(lead.Email));
                    ModelState.Remove(nameof(lead.Phone));
                    ModelState.Remove(nameof(lead.CompanyEn));
                    ModelState.Remove(nameof(lead.CompanyAr));
                    ModelState.Remove(nameof(lead.PotentialValue));

                    // Re-validate the model with updated values
                    TryValidateModel(lead);
                }
            }

            if (ModelState.IsValid)
            {
                lead.CreatedDate = DateTime.Now;
                _context.Add(lead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate lead source dropdown when validation fails
            PopulateLeadSourceList(lead.Source);
            return View(lead);
        }

        // GET: Leads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads
                .Include(l => l.Customer)
                    .ThenInclude(c => c.Agent)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            ViewData["Agents"] = new SelectList(_context.Agents.Where(a => a.IsActive), "Id", "Name", lead.Customer?.AgentId);
            return View(lead);
        }

        // POST: Leads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameEn,NameAr,Email,Phone,CompanyEn,CompanyAr,Source,Status,PotentialValue,Notes,RequestDate,CreatedDate,LastContactDate,AssignedTo,CustomerId")] Lead lead)
        {
            if (id != lead.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadExists(lead.Id))
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
            return View(lead);
        }

        // GET: Leads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lead = await _context.Leads
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (lead != null)
            {
                _context.Leads.Remove(lead);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Leads/CreateCustomerFromLead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomerFromLead(int leadId)
        {
            var lead = await _context.Leads.FindAsync(leadId);
            if (lead == null)
            {
                return NotFound();
            }

            // Create customer from lead data
            var customer = new Customer
            {
                CustomerTypeId = 1, // Default to Individual
                CustomerName = lead.NameEn,
                CustomerNameAr = lead.NameAr,
                Email = lead.Email,
                MobilePhone = lead.Phone,
                Address = lead.CompanyEn,
                AddressAr = lead.CompanyAr,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ConvertedToCustomer = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Link the lead to the customer
            lead.CustomerId = customer.Id;
            _context.Update(lead);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Customer created successfully from lead and linked!";
            return RedirectToAction(nameof(Edit), new { id = leadId });
        }

        // API: Search Customers
        [HttpGet]
        public async Task<IActionResult> SearchCustomers(string query, string searchType)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(searchType))
            {
                return Json(new { success = false, message = "Query and search type are required" });
            }

            var customers = await _context.Customers
                .Include(c => c.Agent)
                .Where(c => c.IsActive)
                .ToListAsync();

            IEnumerable<Customer> filteredCustomers;

            switch (searchType.ToLower())
            {
                case "name":
                    filteredCustomers = customers.Where(c =>
                        c.CustomerName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        (c.CustomerNameAr != null && c.CustomerNameAr.Contains(query, StringComparison.OrdinalIgnoreCase)));
                    break;
                case "phone":
                    filteredCustomers = customers.Where(c =>
                        c.MobilePhone != null && c.MobilePhone.Contains(query));
                    break;
                case "cpr":
                    filteredCustomers = customers.Where(c =>
                        c.CPR_CR_Number != null && c.CPR_CR_Number.Contains(query));
                    break;
                case "agent":
                    filteredCustomers = customers.Where(c =>
                        c.Agent != null && c.Agent.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    return Json(new { success = false, message = "Invalid search type" });
            }

            var results = filteredCustomers
                .Take(10) // Limit results
                .Select(c => new {
                    id = c.Id,
                    name = c.CustomerName,
                    nameAr = c.CustomerNameAr,
                    phone = c.MobilePhone,
                    email = c.Email,
                    cpr = c.CPR_CR_Number,
                    agentName = c.Agent?.Name,
                    agentId = c.AgentId
                });

            return Json(new { success = true, customers = results });
        }

        // API: Get Customer Details
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Agent)
                .Include(c => c.CustomerGroup)
                .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive);

            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found" });
            }

            return Json(new {
                success = true,
                customer = new {
                    id = customer.Id,
                    customerName = customer.CustomerName,
                    customerNameAr = customer.CustomerNameAr,
                    cpr_CR_Number = customer.CPR_CR_Number,
                    mobilePhone = customer.MobilePhone,
                    email = customer.Email,
                    customerGroup = customer.CustomerGroup != null ? new {
                        nameEn = customer.CustomerGroup.GroupName
                    } : null,
                    agentId = customer.AgentId,
                    agentName = customer.Agent?.Name
                }
            });
        }

        // POST: Leads/CreateCustomerFromLead
        [HttpPost]
        public async Task<IActionResult> CreateCustomerFromLead([FromForm] Customer customer)
        {
            try
            {
                // Set default values for new customer
                customer.CreatedDate = DateTime.Now;
                customer.IsActive = true;

                // Set default customer type if not provided
                if (customer.CustomerTypeId == 0)
                {
                    var defaultType = await _context.CustomerTypes.FirstOrDefaultAsync(ct => ct.Name == "Individual");
                    if (defaultType != null)
                    {
                        customer.CustomerTypeId = defaultType.Id;
                    }
                }

                _context.Add(customer);
                await _context.SaveChangesAsync();

                return Json(new { success = true, customerId = customer.Id, message = "Customer created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer from lead");
                return Json(new { success = false, message = "Error creating customer" });
            }
        }

        private void PopulateLeadSourceList(string? selectedSource = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            var sources = _context.LeadSources
                .Where(ls => ls.IsActive)
                .OrderBy(ls => ls.Name)
                .Select(ls => new SelectListItem
                {
                    Value = ls.Name,
                    Text = currentLang == "ar" && !string.IsNullOrEmpty(ls.NameAr) ? ls.NameAr : ls.Name
                })
                .ToList();

            ViewBag.LeadSources = new SelectList(sources, "Value", "Text", selectedSource);
        }

        private bool LeadExists(int id)
        {
            return _context.Leads.Any(e => e.Id == id);
        }
    }
}