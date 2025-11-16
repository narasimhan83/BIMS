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
    public class CustomerPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerPayments
        public async Task<IActionResult> Index()
        {
            var customerPayments = await _context.CustomerPayments
                .Include(c => c.Customer)
                .Include(c => c.SalesInvoice)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(customerPayments);
        }

        // GET: CustomerPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerPayment = await _context.CustomerPayments
                .Include(c => c.Customer)
                .Include(c => c.SalesInvoice)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customerPayment == null)
            {
                return NotFound();
            }

            return View(customerPayment);
        }

        // GET: CustomerPayments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName");
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName");
            return View();
        }

        // POST: CustomerPayments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentReference,CustomerId,SalesInvoiceId,PaymentDate,Amount,PaymentMethod,BankReference,Notes")] CustomerPayment customerPayment)
        {
            if (ModelState.IsValid)
            {
                customerPayment.CreatedDate = DateTime.Now;
                _context.Add(customerPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName", customerPayment.CustomerId);
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", customerPayment.SalesInvoiceId);
            return View(customerPayment);
        }

        // GET: CustomerPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerPayment = await _context.CustomerPayments.FindAsync(id);
            if (customerPayment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName", customerPayment.CustomerId);
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", customerPayment.SalesInvoiceId);
            return View(customerPayment);
        }

        // POST: CustomerPayments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PaymentReference,CustomerId,SalesInvoiceId,PaymentDate,Amount,PaymentMethod,BankReference,Notes,CreatedDate")] CustomerPayment customerPayment)
        {
            if (id != customerPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerPaymentExists(customerPayment.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName", customerPayment.CustomerId);
            ViewData["SalesInvoiceId"] = new SelectList(_context.SalesInvoices
                .Include(si => si.Customer)
                .Select(si => new {
                    Id = si.Id,
                    DisplayName = $"{si.InvoiceNumber} - {si.Customer.CustomerName}"
                }), "Id", "DisplayName", customerPayment.SalesInvoiceId);
            return View(customerPayment);
        }

        // GET: CustomerPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerPayment = await _context.CustomerPayments
                .Include(c => c.Customer)
                .Include(c => c.SalesInvoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerPayment == null)
            {
                return NotFound();
            }

            return View(customerPayment);
        }

        // POST: CustomerPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerPayment = await _context.CustomerPayments.FindAsync(id);
            if (customerPayment != null)
            {
                _context.CustomerPayments.Remove(customerPayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerPaymentExists(int id)
        {
            return _context.CustomerPayments.Any(e => e.Id == id);
        }
    }
}