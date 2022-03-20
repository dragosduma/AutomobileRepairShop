using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AutomobileRepairShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AutoRSContext db = new AutoRSContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //cookie magic x10000 pls dont delete it's holy
            string cookieValueFromContext = HttpContext.Request.Cookies["userSession"];
            // ^^^^ holy line do not delete
            
            //if (cookieValueFromContext != null)
            //    Debug.WriteLine("Name:" +this.User.FindFirst(ClaimTypes.Name).Value);
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
            return RedirectToAction("Login", "Login");
        }

        public IActionResult Register()
        {
            return RedirectToAction("Register","Registration");
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

        public ActionResult AddEmployee()
        {
            return View();
        }

        public ActionResult EditAccounts()
        {
            return View(db.Users.ToList());
        }
    }
}