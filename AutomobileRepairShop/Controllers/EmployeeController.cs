﻿using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private CarPart carPartsList = new CarPart();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            return View(db.CarParts.ToList());
        }
<<<<<<< HEAD

=======
        [HttpPost]
        public JsonResult Bills(string ItemId)
        {
            CarPart carPart = db.CarParts.Single(model => model.Id.ToString() == ItemId);
            carPartsList.partsList.Add(carPart);
            foreach(CarPart cp in carPartsList.partsList) {
                Debug.WriteLine(cp.Name);
            }
            return Json(new {success=true,Counter=carPartsList.partsList.Count});
        }
        public ActionResult Appointments()
        {
            return View();
        }
>>>>>>> e06d4e4f09b48cb0c868a06002dc8e0a29c847c2
    }
}
