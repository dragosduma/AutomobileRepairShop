using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class ControllerBase : Controller
    {
        //verifies is there is a user that is logged in
        public bool IsLogged()
        {
            bool isLogged = false;
            var loggedInUser = HttpContext.User;
            var claym = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (claym != null)
                isLogged = true;
            return isLogged;
        }

        //get name of the logged user
        public string GetName()
        {
            return this.User.FindFirst(ClaimTypes.Name).Value;
        }
        public string GetEmail()
        {
            return this.User.FindFirst(ClaimTypes.Email).Value;
        }

        public string GetRole()
        {
            return this.User.FindFirst(ClaimTypes.Role).Value;
        }

        public bool IsAdmin()
        {
            bool isAdmin = false;
            ClaimsPrincipal loggedInUser = HttpContext.User;
            // claym contains the role of the logged in user

            var claym = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            if (claym != null)
                if (claym.Value.ToString() == "Administrator")
                    isAdmin = true;
            return isAdmin;
        }

        public bool IsEmployee()
        {
            bool isEmployee = false;
            ClaimsPrincipal loggedInUser = HttpContext.User;

            var claym = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            if (claym != null)
                if (claym.Value.ToString() == "Employee")
                    isEmployee = true;

            return isEmployee;
        }

        protected static string HashPassword(string password)
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
