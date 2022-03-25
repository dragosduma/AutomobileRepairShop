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
            AutoRSContext db = new AutoRSContext();
            User u=db.Users.FirstOrDefault(x => x.Id == user.Id);
            if(user.Surname!=null)u.Surname = user.Surname;
            if(user.Name != null) u.Name = user.Name;
            if(user.Email!=null) u.Email= user.Email;
            if(user.Address!=null) u.Address = user.Address;
            if(user.Birthday!=null) u.Birthday = user.Birthday;
            Debug.WriteLine("Name: "+ user.Name+" "+u.Name);
            db.SaveChanges();
            return RedirectToAction("EditAccounts","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount(User user)
        {
            AutoRSContext db = new AutoRSContext();
            db.Remove(user);
            Debug.WriteLine("User " + user.Name + " removed");
            db.SaveChanges();
            return RedirectToAction("EditAccounts", "Home");
        }
    }
}
