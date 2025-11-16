using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class InsurancePlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InsurancePlansController> _logger;

        public InsurancePlansController(ApplicationDbContext context, ILogger<InsurancePlansController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: InsurancePlans
        public async Task<IActionResult> Index()
        {
            var insurancePlans = await _context.InsurancePlans
                .Include(ip => ip.InsuranceClient)
                .OrderByDescending(ip => ip.CreatedDate)
                .ToListAsync();
            return View(insurancePlans);
        }

        // GET: InsurancePlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePlan = await _context.InsurancePlans
                .Include(ip => ip.InsuranceClient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (insurancePlan == null)
            {
                return NotFound();
            }

            return View(insurancePlan);
        }

        // GET: InsurancePlans/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownLists();
            return View();
        }

        // POST: InsurancePlans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceClientId,PlanName,PlanNameAr,Description,DescriptionAr,IsActive")] InsurancePlan insurancePlan)
        {
            if (ModelState.IsValid)
            {
                insurancePlan.CreatedDate = DateTime.UtcNow;
                _context.Add(insurancePlan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance plan created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownLists(insurancePlan.InsuranceClientId);
            return View(insurancePlan);
        }

        // GET: InsurancePlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePlan = await _context.InsurancePlans.FindAsync(id);
            if (insurancePlan == null)
            {
                return NotFound();
            }

            await PopulateDropdownLists(insurancePlan.InsuranceClientId);
            return View(insurancePlan);
        }

        // POST: InsurancePlans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsuranceClientId,PlanName,PlanNameAr,Description,DescriptionAr,IsActive,CreatedDate")] InsurancePlan insurancePlan)
        {
            if (id != insurancePlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    insurancePlan.ModifiedDate = DateTime.UtcNow;
                    _context.Update(insurancePlan);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Insurance plan updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsurancePlanExists(insurancePlan.Id))
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

            await PopulateDropdownLists(insurancePlan.InsuranceClientId);
            return View(insurancePlan);
        }

        // GET: InsurancePlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePlan = await _context.InsurancePlans
                .Include(ip => ip.InsuranceClient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (insurancePlan == null)
            {
                return NotFound();
            }

            return View(insurancePlan);
        }

        // POST: InsurancePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurancePlan = await _context.InsurancePlans.FindAsync(id);
            if (insurancePlan != null)
            {
                _context.InsurancePlans.Remove(insurancePlan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance plan deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InsurancePlanExists(int id)
        {
            return _context.InsurancePlans.Any(e => e.Id == id);
        }

        // Helper method to populate dropdown lists
        private async Task PopulateDropdownLists(int? selectedInsuranceClientId = null)
        {
            // Insurance Clients
            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name", selectedInsuranceClientId);
        }
    }
}