using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private List<CarPart> carPartsList = new List<CarPart>();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            return View(db.CarParts.ToList());
        }

        [HttpPost]
        public JsonResult Bills(string ItemId)
        {
            CarPart carPart = db.CarParts.Single(model => model.Id.ToString() == ItemId);
            carPartsList.Add(carPart);
            foreach(CarPart cp in carPartsList) {
                Debug.WriteLine(cp.Name);
            }
            return Json(new {success=true,Counter=carPartsList.Count});
        }
        public ActionResult Appointments()
        {
            return View();
        }
    }
}
