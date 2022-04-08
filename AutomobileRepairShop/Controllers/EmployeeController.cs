using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            return View(db.CarParts.ToList());
        }
        [HttpPost]
        public JsonResult Bills(string array)
        {
            //IList<long> list = array.Split(',');
            //Debug.WriteLine(array[0]);
            /*foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                
                Debug.WriteLine(carPart.Id);
            }*/
            return Json(new { success = true });
        }
        public ActionResult Appointments()
        {
            return View();
        }
    }
}
