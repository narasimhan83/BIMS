using BIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public IActionResult SetLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang) && (lang == "en" || lang == "ar"))
            {
                HttpContext.Session.SetString("Language", lang);
                _logger.LogInformation($"Language set to: {lang}");
            }
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}