using Microsoft.AspNetCore.Mvc;
using AutomobileRepairShop.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Diagnostics;

namespace AutomobileRepairShop.Controllers
{
    
    public class RegistrationController : Controller
    {
        

        [HttpPost]
        public IActionResult Register(User user)
        {
            // access to the database in order to add a new user
            AutoRSContext db = new AutoRSContext();
            // idrole==2 means regular user
            user.IdRole = 2;

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            string password = user.Password;

            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            user.Password = hashed;
            
            // check for email duplicates

            User email=db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            try
            {
                if(email==null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login","Home");
                }else
                {
                    Debug.WriteLine("Email address already exists");
                    ViewBag.EmailExists= "Email address already exists";
                    ViewBag.UserName = user.Name;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return View();
            //page link changes, i don't like it :(
            //tried return RedirectToRoute(new {controller="Home",action="Register" });
            //but form's textboxes clear
        }
    }
}
