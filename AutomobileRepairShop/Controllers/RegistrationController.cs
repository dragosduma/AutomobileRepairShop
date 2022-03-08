using Microsoft.AspNetCore.Mvc;
using AutomobileRepairShop.Models;

namespace AutomobileRepairShop.Controllers
{
    
    public class RegistrationController : Controller
    {
        

        [HttpPost]
        public IActionResult Welcome(User user)
        {
            AutoRSContext db = new AutoRSContext();

            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;
            string password = user.Password;
            user.Id = 2;
            db.Users.Add(user);
            return View();
        }
    }
}
