using Microsoft.AspNetCore.Mvc;
using AutomobileRepairShop.Models;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    
    public class RegistrationController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // access to the database in order to add a new user
            AutoRSContext db = new AutoRSContext();
            // idrole==2 means regular user
            user.IdRole = 2;
<<<<<<< HEAD
            
            // check for email duplicates

            User email=db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            try
            {
                if(email==null)
=======

            // check for email duplicates

            User email = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            try
            {
                if (email == null)
>>>>>>> 6648fa39247a8067439f33eadba0cfc396cff235
                {
                    user.Password = HashPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
<<<<<<< HEAD
                    return RedirectToAction("Login","Home");
                }else
                {
                    Debug.WriteLine("Email address already exists");
                    ViewBag.EmailExists= "Email address already exists";
                    ViewBag.UserName = user.Name;
                }
            }
            catch(Exception ex)
=======
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    Debug.WriteLine("Email address already exists");
                    ViewBag.EmailExists = "Email address already exists";
                    ViewBag.UserName = user.Name;
                }
            }
            catch (Exception ex)
>>>>>>> 6648fa39247a8067439f33eadba0cfc396cff235
            {
                Debug.WriteLine(ex.ToString());
            }
            return View();

        }
        private static string HashPassword(string password)
        {
            string hashPass = string.Empty;

            // 1.-Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[20]);

            // 2.-Create the RFC2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // 3.-Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[40];
            Array.Copy(salt, 0, hashBytes, 0, 20);
            Array.Copy(hash, 0, hashBytes, 20, 20);

            // 4.-Turn the combined salt+hash into a string for storage
            hashPass = Convert.ToBase64String(hashBytes);
            return hashPass;
        }
    }
}
