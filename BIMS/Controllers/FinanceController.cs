using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class FinanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FinanceController> _logger;

        public FinanceController(ApplicationDbContext context, ILogger<FinanceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============================================
        // BANK CRUD OPERATIONS
        // ============================================

        // GET: Finance/Banks
        public async Task<IActionResult> Banks()
        {
            var banks = await _context.Banks
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
            return View(banks);
        }

        // GET: Finance/CreateBank
        public IActionResult CreateBank()
        {
            return View();
        }

        // POST: Finance/CreateBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBank([Bind("Name,NameAr,Code,Description,DescriptionAr,IBAN,SwiftCode,AccountNumber,BranchName,BranchNameAr,Address,AddressAr,ContactPhone,ContactEmail,IsActive")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                bank.CreatedDate = DateTime.UtcNow;
                bank.CreatedBy = User.Identity?.Name;
                _context.Add(bank);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Bank created successfully!";
                return RedirectToAction(nameof(Banks));
            }
            return View(bank);
        }

        // GET: Finance/EditBank/5
        public async Task<IActionResult> EditBank(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }

        // POST: Finance/EditBank/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBank(int id, [Bind("Id,Name,NameAr,Code,Description,DescriptionAr,IBAN,SwiftCode,AccountNumber,BranchName,BranchNameAr,Address,AddressAr,ContactPhone,ContactEmail,IsActive,CreatedDate,CreatedBy")] Bank bank)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bank.ModifiedDate = DateTime.UtcNow;
                    bank.ModifiedBy = User.Identity?.Name;
                    _context.Update(bank);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Bank updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankExists(bank.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Banks));
            }
            return View(bank);
        }

        // GET: Finance/DeleteBank/5
        public async Task<IActionResult> DeleteBank(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.Banks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // POST: Finance/DeleteBankConfirmed/5
        [HttpPost, ActionName("DeleteBank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBankConfirmed(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Bank deleted successfully!";
            }
            return RedirectToAction(nameof(Banks));
        }

        private bool BankExists(int id)
        {
            return _context.Banks.Any(e => e.Id == id);
        }
    }
}