using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace BIMS.Controllers
{
    [Authorize]
    public class PoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Policies
        public async Task<IActionResult> Index()
        {
            var policies = await _context.Policies
                .Include(p => p.InsuranceClient)
                .Include(p => p.ProductType)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
            return View(policies);
        }

        // GET: Policies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.InsuranceClient)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // GET: Policies/Create
        public IActionResult Create()
        {
            ViewData["InsuranceClientId"] = new SelectList(_context.InsuranceClients, "Id", "NameEn");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "NameEn");
            return View();
        }

        // POST: Policies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PolicyNumber,InsuranceClientId,ProductTypeId,StartDate,EndDate,PremiumAmount,Status,CoverageDetailsEn,CoverageDetailsAr,SumInsured,TermsAndConditionsEn,TermsAndConditionsAr")] Policy policy)
        {
            if (ModelState.IsValid)
            {
                policy.CreatedDate = DateTime.Now;
                _context.Add(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InsuranceClientId"] = new SelectList(_context.InsuranceClients, "Id", "NameEn", policy.InsuranceClientId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "NameEn", policy.ProductTypeId);
            return View(policy);
        }

        // GET: Policies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            ViewData["InsuranceClientId"] = new SelectList(_context.InsuranceClients, "Id", "NameEn", policy.InsuranceClientId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "NameEn", policy.ProductTypeId);
            return View(policy);
        }

        // POST: Policies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PolicyNumber,InsuranceClientId,ProductTypeId,StartDate,EndDate,PremiumAmount,Status,CoverageDetailsEn,CoverageDetailsAr,SumInsured,TermsAndConditionsEn,TermsAndConditionsAr,CreatedDate")] Policy policy)
        {
            if (id != policy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.Id))
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
            ViewData["InsuranceClientId"] = new SelectList(_context.InsuranceClients, "Id", "NameEn", policy.InsuranceClientId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "NameEn", policy.ProductTypeId);
            return View(policy);
        }

        // GET: Policies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.InsuranceClient)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Policies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }
    }
}