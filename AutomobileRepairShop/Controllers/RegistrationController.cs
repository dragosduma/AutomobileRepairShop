using Microsoft.AspNetCore.Mvc;
using AutomobileRepairShop.Models;

namespace AutomobileRepairShop.Controllers
{
    
    public class RegistrationController : Controller
    {
        

        [HttpPost]
        public IActionResult Login(User user)
        {
            // access to the database in order to add a new user
            AutoRSContext db = new AutoRSContext();
            // idrole==2 means regular user
            user.IdRole = 2;
            // password hashing

            // check for email duplicates


            // adds user 
            db.Users.Add(user);
            db.SaveChanges();
            return View();
        }
    }
}
