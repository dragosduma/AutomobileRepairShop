using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Dynamic;

namespace AutomobileRepairShop.Controllers
{
    public class ClientController : ControllerBase
    {
        private List<AppointClasses> appointClasses = new List<AppointClasses>();
        private List<Appointment> appointments = new List<Appointment>();

        private AutoRSContext db = new AutoRSContext();

        private dynamic mymodel = new ExpandoObject();


        [Authorize(Roles = "Customer")]
        public ActionResult History()
        {
            String email = GetEmail();
            User user = db.Users.FirstOrDefault(x => x.Email == email);
            appointments = db.Appointments.ToList();
            Debug.WriteLine(user.Id);
            if (user == null)
            {
                ViewBag.Message = "User doesn't exist";
            }
            else
            {
                Debug.WriteLine("User gasit");
                bool found = false;
                appointClasses.Clear();
                foreach (Appointment app in appointments)
                {
                    if (app.IdUser == user.Id && app.Finished == true)
                    {
                        found = true;
                        Car car = db.Cars.FirstOrDefault(model => model.Id == app.IdCar);
                        Bill bill = db.Bills.FirstOrDefault(model => model.AppointmentId == app.Id);
                        AppointClasses thisapp = new AppointClasses(app, car, user, bill.Price, bill.Id);
                        appointClasses.Add(thisapp);
                        Debug.WriteLine(thisapp.Appointment.Id);
                    }
                }
            }
            mymodel.AppointList = appointClasses;
            ViewBag.IsLogged = IsLogged();
            return View(mymodel);
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
            int numberofapp = db.Appointments.Count(date => date.Date == appointClasses.Appointment.Date);
            if (numberofapp > 4)
            {
                ViewBag.noa = "Date unavailable!";
                return View("./Appointments", appointClasses);
            }

            String email = GetEmail();
            User user = db.Users.FirstOrDefault(x => x.Email == email);
            Car car = db.Cars.FirstOrDefault(x => x.ChassisCode == appointClasses.Car.ChassisCode);

            if (car == null)
            {
                car = appointClasses.Car;
                car.IdUser = user.Id;
                db.Cars.Add(car);
                db.SaveChanges();
            }
            else if (car.IdUser != user.Id)
            {
                ViewBag.car = "Not your car!";
                return View("./Appointments", appointClasses);
            }
            Appointment app = appointClasses.Appointment;
            app.IdUser = user.Id;
            app.IdCar = car.Id;
            app.Finished = false;
            db.Appointments.Add(app);
            car.IdUser = user.Id;
            car.Kilometers = appointClasses.Car.Kilometers;
            db.Cars.Update(car);
            db.SaveChanges();

            return RedirectToAction("Index","Home");  //maybe redirect to user's appointment list/history
        }


    }
}
