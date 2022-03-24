using Microsoft.AspNetCore.Mvc;
using AutomobileRepairShop.Models;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    public class EditAccountController : Controller
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount (User user)
        {
            //AutoRSContext db = new AutoRSContext();
            //User u=db.Users.FirstOrDefault(x => x.Id == user.Id);
            Debug.WriteLine("Id: "+ user.Id);
            return RedirectToAction("EditAccounts","Home");
        }
        [HttpGet]
        public IActionResult DeleteAccount(int id)
        {
            AutoRSContext db = new AutoRSContext();
            User u = db.Users.FirstOrDefault(x => x.Id == id);
            return PartialView("EditAccounts",u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount(User user)
        {
            AutoRSContext db = new AutoRSContext();
            db.Remove(user);
            db.SaveChanges();
            return RedirectToAction("EditAccounts", "Home");
        }
    }
}
