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
                .Include(ip => ip.LineOfBusiness)
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
                .Include(ip => ip.LineOfBusiness)
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
        public async Task<IActionResult> Create([Bind("InsuranceClientId,LineOfBusinessId,PlanName,PlanNameAr,PlanCode,PlanTier,LaunchDate,WithdrawDate,Description,DescriptionAr,IsActive")] InsurancePlan insurancePlan)
        {
            // Ensure selected Line of Business belongs to the chosen Insurance Client
            if (insurancePlan.InsuranceClientId == 0)
            {
                ModelState.AddModelError("InsuranceClientId", "Insurance Client is required");
            }
            else
            {
                var isValidLOB = await _context.LinesOfBusiness
                    .AnyAsync(l => l.Id == insurancePlan.LineOfBusinessId
                                   && l.InsuranceClientId == insurancePlan.InsuranceClientId
                                   && l.IsActive);
                if (!isValidLOB)
                {
                    ModelState.AddModelError("LineOfBusinessId", "Selected Line of Business is not valid for the chosen Insurance Client.");
                }
            }

            if (ModelState.IsValid)
            {
                insurancePlan.CreatedDate = DateTime.UtcNow;
                _context.Add(insurancePlan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance plan created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownLists(insurancePlan.InsuranceClientId, insurancePlan.LineOfBusinessId);
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

            await PopulateDropdownLists(insurancePlan.InsuranceClientId, insurancePlan.LineOfBusinessId);
            return View(insurancePlan);
        }

        // POST: InsurancePlans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsuranceClientId,LineOfBusinessId,PlanName,PlanNameAr,PlanCode,PlanTier,LaunchDate,WithdrawDate,Description,DescriptionAr,IsActive,CreatedDate")] InsurancePlan insurancePlan)
        {
            if (id != insurancePlan.Id)
            {
                return NotFound();
            }

            // Ensure selected Line of Business belongs to the chosen Insurance Client
            if (insurancePlan.InsuranceClientId == 0)
            {
                ModelState.AddModelError("InsuranceClientId", "Insurance Client is required");
            }
            else
            {
                var isValidLOB = await _context.LinesOfBusiness
                    .AnyAsync(l => l.Id == insurancePlan.LineOfBusinessId
                                   && l.InsuranceClientId == insurancePlan.InsuranceClientId
                                   && l.IsActive);
                if (!isValidLOB)
                {
                    ModelState.AddModelError("LineOfBusinessId", "Selected Line of Business is not valid for the chosen Insurance Client.");
                }
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

            await PopulateDropdownLists(insurancePlan.InsuranceClientId, insurancePlan.LineOfBusinessId);
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
        private async Task PopulateDropdownLists(int? selectedInsuranceClientId = null, int? selectedLineOfBusinessId = null)
        {
            // Insurance Clients
            var insuranceClients = await _context.InsuranceClients
                .Where(ic => ic.IsActive)
                .OrderBy(ic => ic.Name)
                .ToListAsync();
            ViewBag.InsuranceClients = new SelectList(insuranceClients, "Id", "Name", selectedInsuranceClientId);

            // Lines of Business filtered by selected Insurance Client
            var lobsQuery = _context.LinesOfBusiness.AsQueryable();
            if (selectedInsuranceClientId.HasValue && selectedInsuranceClientId.Value > 0)
            {
                lobsQuery = lobsQuery.Where(l => l.IsActive && l.InsuranceClientId == selectedInsuranceClientId.Value);
            }
            else
            {
                lobsQuery = lobsQuery.Where(l => false); // empty list when no client selected
            }

            var lobs = await lobsQuery
                .OrderBy(l => l.Name)
                .ToListAsync();

            ViewBag.LinesOfBusiness = new SelectList(lobs, "Id", "Name", selectedLineOfBusinessId);
        }

        // AJAX: return LOBs for a given Insurance Client
        [HttpGet]
        public async Task<IActionResult> GetLinesOfBusiness(int insuranceClientId)
        {
            var lobs = await _context.LinesOfBusiness
                .Where(l => l.IsActive && l.InsuranceClientId == insuranceClientId)
                .OrderBy(l => l.Name)
                .Select(l => new { id = l.Id, name = l.Name, nameAr = l.NameAr })
                .ToListAsync();

            return Json(lobs);
        }
    }
}