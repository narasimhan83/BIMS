using BIMS.Data;
using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Controllers
{
    [Authorize]
    public class PlanNetworksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanNetworksController> _logger;

        public PlanNetworksController(ApplicationDbContext context, ILogger<PlanNetworksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PlanNetworks
        public async Task<IActionResult> Index()
        {
            var items = await _context.PlanNetworks
                .AsNoTracking()
                .OrderByDescending(pn => pn.CreatedDate)
                .Select(pn => new PlanNetwork
                {
                    Id = pn.Id,
                    InsurancePlanId = pn.InsurancePlanId,
                    ProviderNetworkId = pn.ProviderNetworkId,
                    IsActive = pn.IsActive,
                    CreatedDate = pn.CreatedDate,
                    ModifiedDate = pn.ModifiedDate,

                    InsurancePlan = pn.InsurancePlan == null
                        ? null
                        : new InsurancePlan
                        {
                            Id = pn.InsurancePlan.Id,
                            PlanName = pn.InsurancePlan.PlanName,
                            PlanNameAr = pn.InsurancePlan.PlanNameAr
                        },

                    ProviderNetwork = pn.ProviderNetwork == null
                        ? null
                        : new ProviderNetwork
                        {
                            Id = pn.ProviderNetwork.Id,
                            NetworkName = pn.ProviderNetwork.NetworkName,
                            IsActive = pn.ProviderNetwork.IsActive
                        }
                })
                .ToListAsync();

            return View("~/Views/PlanNetworks/Index.cshtml", items);
        }

        // GET: PlanNetworks/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View("~/Views/PlanNetworks/Create.cshtml");
        }

        // POST: PlanNetworks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsurancePlanId,ProviderNetworkId,IsActive")] PlanNetwork model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity?.Name;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan network created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.ProviderNetworkId);
            return View("~/Views/PlanNetworks/Create.cshtml", model);
        }

        // GET: PlanNetworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanNetworks.FindAsync(id);
            if (entity == null)
                return NotFound();

            await PopulateDropdowns(entity.InsurancePlanId, entity.ProviderNetworkId);
            return View("~/Views/PlanNetworks/Edit.cshtml", entity);
        }

        // POST: PlanNetworks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsurancePlanId,ProviderNetworkId,IsActive,CreatedDate,CreatedBy")] PlanNetwork model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifiedDate = DateTime.UtcNow;
                    model.ModifiedBy = User.Identity?.Name;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Plan network updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanNetworkExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model.InsurancePlanId, model.ProviderNetworkId);
            return View("~/Views/PlanNetworks/Edit.cshtml", model);
        }

        // GET: PlanNetworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = await _context.PlanNetworks
                .AsNoTracking()
                .Include(pn => pn.InsurancePlan)
                .Include(pn => pn.ProviderNetwork)
                .FirstOrDefaultAsync(pn => pn.Id == id);

            if (entity == null)
                return NotFound();

            return View("~/Views/PlanNetworks/Delete.cshtml", entity);
        }

        // POST: PlanNetworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.PlanNetworks.FindAsync(id);
            if (entity != null)
            {
                _context.PlanNetworks.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plan network deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanNetworkExists(int id)
        {
            return _context.PlanNetworks.Any(e => e.Id == id);
        }

        private async Task PopulateDropdowns(
            int? selectedPlanId = null,
            int? selectedProviderNetworkId = null)
        {
            var currentLang = HttpContext.Session.GetString("Language") ?? "en";

            // Insurance Plans
            var planItems = await _context.InsurancePlans
                .OrderBy(p => p.PlanName)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = currentLang == "ar" && !string.IsNullOrEmpty(p.PlanNameAr)
                        ? p.PlanNameAr
                        : p.PlanName
                })
                .ToListAsync();

            if (selectedPlanId.HasValue)
            {
                foreach (var item in planItems)
                {
                    if (item.Value == selectedPlanId.Value.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            ViewBag.InsurancePlans = planItems;

            // Provider Networks
            var networks = await _context.ProviderNetworks
                .OrderBy(n => n.NetworkName)
                .ToListAsync();

            ViewBag.ProviderNetworks = new SelectList(
                networks,
                "Id",
                nameof(ProviderNetwork.NetworkName),
                selectedProviderNetworkId);
        }
    }
}