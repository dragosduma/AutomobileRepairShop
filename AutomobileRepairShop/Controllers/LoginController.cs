using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class LoginController : ControllerBase
    {
        // LOGIN RELATED METHODS
        public IActionResult Login()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(User user)
        {
            if (user.Email == null || user.Password == null) 
                return View();
            AutoRSContext db = new AutoRSContext();

            User user1 = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            if (user1 == null)
            {
                ViewBag.EmailNotExists = "Account doesn't exist!";
                return View();
            }
            if (!VerifyPassword(user.Password, user1.Password))
            {
                ViewBag.WrongPassword = "Wrong password!";
                return View();
            }
            else
            {
                Debug.WriteLine("Welcome " + user1.Name + " " + user1.Surname);

                // cookie magic
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(cookieMagic(user1)),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(7)
                    });
            }
            return Redirect("/");
        }

        private ClaimsIdentity cookieMagic(User user)
        {
            string role;
            switch (user.IdRole)
            {
                case 1: role = "Administrator"; break;
                case 2: role = "Customer"; break;
                case 3: role = "Employee"; break;
                default: role = ""; break;
            }
            Debug.WriteLine(role);
            // fields that the cookie contains using Identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
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

        // LOGOUT RELATED METHODS
        public async Task<IActionResult> LogoutAsync()
        {
            if (Request.Cookies["userSession"] != null)
            {
                Response.Cookies.Delete("userSession");
            }
            Debug.WriteLine(SignOut().ToString() as string);
            return Redirect("/");
        }

        // REGISTRATION RELATED METHODS
        public IActionResult Register()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // access to the database in order to add a new user
            AutoRSContext db = new AutoRSContext();
            // idrole==2 means regular user
            user.IdRole = 2;
            // check for email duplicates
            User email = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            try
            {
                if (email == null)
                {
                    user.Password = HashPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    Debug.WriteLine("Email address already exists");
                    ViewBag.EmailExists = "Email address already exists";
                    ViewBag.UserName = user.Name;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return View();
        }
    }
}
