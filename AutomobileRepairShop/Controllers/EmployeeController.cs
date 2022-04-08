using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    }
}
