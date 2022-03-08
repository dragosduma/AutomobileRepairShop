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
            // check for email duplicates

            // password hashing


            // add user operation
            db.Users.Add(user);
            db.SaveChanges();
            return View();
        }
    }
}
