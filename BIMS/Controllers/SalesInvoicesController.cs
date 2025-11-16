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
    public class SalesInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SalesInvoicesController> _logger;

        public SalesInvoicesController(ApplicationDbContext context, ILogger<SalesInvoicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: SalesInvoices
        public async Task<IActionResult> Index()
        {
            var salesInvoices = await _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
            return View(salesInvoices);
        }

        // GET: SalesInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .Include(s => s.CreditNotes)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // GET: SalesInvoices/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "NameEn");
            return View();
        }

        // POST: SalesInvoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesInvoice salesInvoice)
        {
            _logger.LogInformation("Creating sales invoice");

            if (ModelState.IsValid)
            {
                try
                {
                    // Parse items from form manually
                    var form = HttpContext.Request.Form;
                    var items = new List<SalesInvoiceItem>();

                    // Get all item indices
                    var itemIndices = form.Keys
                        .Where(k => k.StartsWith("items[") && k.Contains("].DescriptionEn"))
                        .Select(k => k.Split('[')[1].Split(']')[0])
                        .Distinct()
                        .ToList();

                    foreach (var index in itemIndices)
                    {
                        var descriptionEn = form[$"items[{index}].DescriptionEn"].ToString();
                        var descriptionAr = form[$"items[{index}].DescriptionAr"].ToString();
                        var quantityStr = form[$"items[{index}].Quantity"].ToString();
                        var unitPriceStr = form[$"items[{index}].UnitPrice"].ToString();

                        if (!string.IsNullOrEmpty(descriptionEn) && !string.IsNullOrEmpty(descriptionAr) &&
                            int.TryParse(quantityStr, out var quantity) &&
                            decimal.TryParse(unitPriceStr, out var unitPrice))
                        {
                            var totalPrice = quantity * unitPrice;
                            items.Add(new SalesInvoiceItem
                            {
                                DescriptionEn = descriptionEn,
                                DescriptionAr = descriptionAr,
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                TotalPrice = totalPrice
                            });
                        }
                    }

                    salesInvoice.Items = items;
                    salesInvoice.CreatedDate = DateTime.Now;

                    // Calculate totals
                    salesInvoice.TotalAmount = items.Sum(i => i.TotalPrice);
                    salesInvoice.NetAmount = salesInvoice.TotalAmount + salesInvoice.TaxAmount - salesInvoice.DiscountAmount;

                    _context.Add(salesInvoice);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Sales invoice created successfully with {items.Count} items");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating sales invoice");
                    ModelState.AddModelError("", "An error occurred while creating the sales invoice.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                }
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "NameEn", salesInvoice.CustomerId);
            return View(salesInvoice);
        }

        // GET: SalesInvoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salesInvoice == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "NameEn", salesInvoice.CustomerId);
            return View(salesInvoice);
        }

        // POST: SalesInvoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesInvoice salesInvoice)
        {
            if (id != salesInvoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Parse items from form manually
                    var form = HttpContext.Request.Form;
                    var items = new List<SalesInvoiceItem>();

                    // Get all item indices
                    var itemIndices = form.Keys
                        .Where(k => k.StartsWith("items[") && k.Contains("].DescriptionEn"))
                        .Select(k => k.Split('[')[1].Split(']')[0])
                        .Distinct()
                        .ToList();

                    foreach (var index in itemIndices)
                    {
                        var descriptionEn = form[$"items[{index}].DescriptionEn"].ToString();
                        var descriptionAr = form[$"items[{index}].DescriptionAr"].ToString();
                        var quantityStr = form[$"items[{index}].Quantity"].ToString();
                        var unitPriceStr = form[$"items[{index}].UnitPrice"].ToString();

                        if (!string.IsNullOrEmpty(descriptionEn) && !string.IsNullOrEmpty(descriptionAr) &&
                            int.TryParse(quantityStr, out var quantity) &&
                            decimal.TryParse(unitPriceStr, out var unitPrice))
                        {
                            var totalPrice = quantity * unitPrice;
                            items.Add(new SalesInvoiceItem
                            {
                                DescriptionEn = descriptionEn,
                                DescriptionAr = descriptionAr,
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                TotalPrice = totalPrice
                            });
                        }
                    }

                    // Remove existing items and add new ones
                    var existingItems = await _context.SalesInvoiceItems.Where(i => i.SalesInvoiceId == id).ToListAsync();
                    _context.SalesInvoiceItems.RemoveRange(existingItems);

                    salesInvoice.Items = items;

                    // Calculate totals
                    salesInvoice.TotalAmount = items.Sum(i => i.TotalPrice);
                    salesInvoice.NetAmount = salesInvoice.TotalAmount + salesInvoice.TaxAmount - salesInvoice.DiscountAmount;

                    _context.Update(salesInvoice);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesInvoiceExists(salesInvoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "NameEn", salesInvoice.CustomerId);
            return View(salesInvoice);
        }

        // GET: SalesInvoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // POST: SalesInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesInvoice = await _context.SalesInvoices.FindAsync(id);
            if (salesInvoice != null)
            {
                _context.SalesInvoices.Remove(salesInvoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesInvoiceExists(int id)
        {
            return _context.SalesInvoices.Any(e => e.Id == id);
        }
    }
}