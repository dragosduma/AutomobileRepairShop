using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User obj)
        {
            return View(obj);
        }
    }
}