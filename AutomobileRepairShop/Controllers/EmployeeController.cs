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
        private List<Appointment> appointList = new List<Appointment>();
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
;           return View(mymodel);
        }
        
        [HttpPost]
        public JsonResult BillsAdd([FromBody]List<CarPart> array)
        {
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                carParts.Add(carPart);
                Debug.Write(carPart.Id+" ");
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
            return RedirectToAction("Bills","Employee") ;

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
            foreach(User u in users)
            {
                if (u.Email == userMail.Email)
                {
                    Debug.WriteLine("User gasit");
                    int idUser = u.Id;
                    foreach(Appointment app in appointments)
                    {
                        if(app.IdUser == idUser && app.Finished == false)
                        {
                            appointList.Add(app);
                            Debug.Write("App gasit");
                        }
                    }

                }
            }
            foreach(Appointment app in appointList)
            {
                Debug.Write("App gasit" + app.Id + " " + app.IdCar + " " + app.IdUser);
            }
            mymodel.AppointList = appointList;
            return RedirectToAction("Bills","Employee");
        }
     
    }
}
