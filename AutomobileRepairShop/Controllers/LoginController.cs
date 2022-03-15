using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
<<<<<<< HEAD
        public IActionResult Login(User user,string sessionName)
=======
        public async Task<IActionResult> LoginAsync(User user)
>>>>>>> 6648fa39247a8067439f33eadba0cfc396cff235
        {
            AutoRSContext db = new AutoRSContext();
            User user1 = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            if (user1 == null)
            {
                ViewBag.EmailNotExists = "Account doesn't exists!";
                return View();
            }

<<<<<<< HEAD
            if (!VerifyPassword(user.Password,user1.Password))
            {
                ViewBag.WrongPassword = "Wrong password";
                return View();
            }else
            {
                Debug.WriteLine("Hello " + user1.Name + " " + user1.Surname);
            }
            return View();
=======
            if (!VerifyPassword(user.Password, user1.Password))
            {
                ViewBag.WrongPassword = "Wrong password ****!";
                return View();
            }
            else
            {
                Debug.WriteLine("Welcome " + user1.Name + " " + user1.Surname);

                // cookie magic
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(cookieMagic(user1)));
                
            }
            return Redirect("/");
        }

        private ClaimsIdentity cookieMagic(User user)
        {
            // fields that the cookie contains using Identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, Convert.ToString(user.IdRole)),  
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
>>>>>>> 6648fa39247a8067439f33eadba0cfc396cff235
        }

        private static bool VerifyPassword(string loginPassword, string dbPassword)
        {
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(dbPassword);
            // Get the salt
            byte[] salt = new byte[20];
            Array.Copy(hashBytes, 0, salt, 0, 20);
            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(loginPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            // compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 20] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
