using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class LoginnController : Controller
    {
        [HttpPost]
        public IActionResult Login(User user)
        {
            AutoRSContext db = new AutoRSContext();
            User user1 = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            if (user1 == null)
            {
                ViewBag.EmailNotExists = "Account doesn't exists!";
                return View();
            }
            bool verifyPassword = true;

            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(user1.Password);
            // Get the salt
            byte[] salt = new byte[20];
            Array.Copy(hashBytes, 0, salt, 0, 20);
            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            // compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 20] != hash[i])
                {
                    verifyPassword = false;
                }
            }
            if (!verifyPassword)
            {
                ViewBag.WrongPassword = "Wrong password";
                return View();
            }else Debug.WriteLine("Welcome "+user1.Name+" "+user1.Surname);
            return View();
        }
    }
}
