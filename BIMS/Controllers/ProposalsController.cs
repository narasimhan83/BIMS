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
    public class ProposalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProposalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proposals
        public async Task<IActionResult> Index()
        {
            var proposals = await _context.Proposals
                .Include(p => p.Lead)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
            return View(proposals);
        }

        // GET: Proposals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals
                .Include(p => p.Lead)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        // GET: Proposals/Create
        public IActionResult Create(int? leadId)
        {
            ViewData["LeadId"] = new SelectList(_context.Leads, "Id", "NameEn", leadId);
            return View();
        }

        // POST: Proposals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LeadId,TitleEn,TitleAr,DescriptionEn,DescriptionAr,Amount,Status,ResponseNotes,ValidUntil")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                proposal.CreatedDate = DateTime.Now;
                _context.Add(proposal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeadId"] = new SelectList(_context.Leads, "Id", "NameEn", proposal.LeadId);
            return View(proposal);
        }

        // GET: Proposals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            ViewData["LeadId"] = new SelectList(_context.Leads, "Id", "NameEn", proposal.LeadId);
            return View(proposal);
        }

        // POST: Proposals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LeadId,TitleEn,TitleAr,DescriptionEn,DescriptionAr,Amount,Status,CreatedDate,SentDate,ResponseDate,ResponseNotes,ValidUntil")] Proposal proposal)
        {
            if (id != proposal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proposal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProposalExists(proposal.Id))
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
            ViewData["LeadId"] = new SelectList(_context.Leads, "Id", "NameEn", proposal.LeadId);
            return View(proposal);
        }

        // GET: Proposals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposal = await _context.Proposals
                .Include(p => p.Lead)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        // POST: Proposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                _context.Proposals.Remove(proposal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProposalExists(int id)
        {
            return _context.Proposals.Any(e => e.Id == id);
        }
    }
}