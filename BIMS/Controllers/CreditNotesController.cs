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
    public class CreditNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreditNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CreditNotes
        public async Task<IActionResult> Index()
        {
            var creditNotes = await _context.CreditNotes
                .Include(c => c.SalesInvoice)
                    .ThenInclude(si => si.Customer)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(creditNotes);
        }

        // GET: CreditNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditNote = await _context.CreditNotes
                .Include(c => c.SalesInvoice)
                    .ThenInclude(si => si.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (creditNote == null)
            {
                return NotFound();
            }

            return View(creditNote);
        }

        // GET: CreditNotes/Create
        public IActionResult Create(int? salesInvoiceId)
        {
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", salesInvoiceId);
            return View();
        }

        // POST: CreditNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreditNoteNumber,SalesInvoiceId,CreditNoteDate,Amount,ReasonEn,ReasonAr,Status")] CreditNote creditNote)
        {
            if (ModelState.IsValid)
            {
                creditNote.CreatedDate = DateTime.Now;
                _context.Add(creditNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", creditNote.SalesInvoiceId);
            return View(creditNote);
        }

        // GET: CreditNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditNote = await _context.CreditNotes.FindAsync(id);
            if (creditNote == null)
            {
                return NotFound();
            }
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", creditNote.SalesInvoiceId);
            return View(creditNote);
        }

        // POST: CreditNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreditNoteNumber,SalesInvoiceId,CreditNoteDate,Amount,ReasonEn,ReasonAr,Status,CreatedDate")] CreditNote creditNote)
        {
            if (id != creditNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditNoteExists(creditNote.Id))
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
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", creditNote.SalesInvoiceId);
            return View(creditNote);
        }

        // GET: CreditNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditNote = await _context.CreditNotes
                .Include(c => c.SalesInvoice)
                    .ThenInclude(si => si.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditNote == null)
            {
                return NotFound();
            }

            return View(creditNote);
        }

        // POST: CreditNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditNote = await _context.CreditNotes.FindAsync(id);
            if (creditNote != null)
            {
                _context.CreditNotes.Remove(creditNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditNoteExists(int id)
        {
            return _context.CreditNotes.Any(e => e.Id == id);
        }
    }
}