using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private List<CarPart> carParts = new List<CarPart>();
        private List<User> users = new List<User>();
        private List<Appointment> appointments = new List<Appointment>();
        private static List<Appointment> appointList = new List<Appointment>();
        private static List<AppointClasses> appointClasses = new List<AppointClasses>();
        private List<Car> carsList = new List<Car>();
        private dynamic mymodel = new ExpandoObject();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
            mymodel.AppointList = appointList;
            ; return View(mymodel);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult CarParts()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
            mymodel.AppointList = appointClasses;
            Debug.WriteLine("CarParts Method");
            return View(mymodel);
        }

        [HttpPost]
        public JsonResult BillsAdd([FromBody] List<CarPart> array)
        {
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                carParts.Add(carPart);
                Debug.Write(carPart.Id + " ");
            }
            Debug.WriteLine("");
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult CreateBills([FromBody] List<CarPart> array)
        {
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);

                Debug.Write(carPart.Id + " ");
            }
            Debug.WriteLine("");
            return RedirectToAction("Bills", "Employee");
        }

        public ActionResult Appointments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchEmail([FromBody] User userMail)
        {
            users = db.Users.ToList();
            appointments = db.Appointments.ToList();

            Debug.WriteLine("SearchEmail entered");
            Debug.WriteLine(userMail.Email);
            User u = db.Users.Single(model => model.Email == userMail.Email);

            Debug.WriteLine("User gasit");

            foreach (Appointment app in appointments)
            {
                if (app.IdUser == u.Id && app.Finished == false)
                {
                    Car car = db.Cars.Single(model => model.Id == app.IdCar);
                    AppointClasses thisapp = new AppointClasses(app, car, u);
                    appointClasses.Add(thisapp);
                    Debug.Write("App gasit:" + thisapp.User.Name + " " + thisapp.Car.Brand + " " + thisapp.Appointment.Date);
                }
            }

            return new EmptyResult();

        }
    }
}
