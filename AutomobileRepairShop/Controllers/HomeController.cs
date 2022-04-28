using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private AutoRSContext db = new AutoRSContext();
        // HOME RELATED METHODS
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // console checker for user login
            ViewBag.IsLogged = IsLogged();
            //Debug.WriteLine(ViewBag.IsLogged.ToString() as string);
            ViewBag.IsAdmin = IsAdmin();
            //Debug.WriteLine(ViewBag.IsAdmin.ToString() as string);
            ViewBag.IsEmployee = IsEmployee();
            return View();
        }


        public IActionResult Privacy()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult Welcome()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [HttpPost]
        public ActionResult Index(User obj)
        {
            return View(obj);
        }

        
        [Authorize(Roles = "Customer")]
        public ActionResult Appointments(AppointClasses appointClasses)
        {
            ViewBag.IsLogged = IsLogged();
            return View(appointClasses);
        }

        [HttpPost]
        public ActionResult AddAppoint(AppointClasses appointClasses)
        {
            String email=GetEmail();
            User user = db.Users.FirstOrDefault(x => x.Email == email);
            Car car = db.Cars.FirstOrDefault(x => x.ChassisCode==appointClasses.Car.ChassisCode);
            if(car == null)
            {
                car = appointClasses.Car;
                car.IdUser = user.Id;
                db.Cars.Add(car);
                db.SaveChanges();
            }
            Appointment app =appointClasses.Appointment;
            app.IdUser = user.Id;
            app.IdCar = car.Id;
            app.Finished = false;
            db.Appointments.Add(app);
            db.SaveChanges();

            return RedirectToAction("Index");  //maybe redirect to user's appointment list/history
        }

    }
}
