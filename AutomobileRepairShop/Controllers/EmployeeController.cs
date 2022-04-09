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
        private dynamic mymodel = new ExpandoObject();
        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();   
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
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
        public ActionResult Appointments()
        {
            return View();
        }
    }
}
