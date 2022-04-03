using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutomobileRepairShop.Controllers
{
    public class AdminController : ControllerBase
    {
        // ADMIN PANEL RELATED METHODS
        private AutoRSContext db = new AutoRSContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult AddEmployee()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsAdmin = IsAdmin();    //unnecessary
            Debug.WriteLine(ViewBag.IsAdmin.ToString() as string);
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult EditAccounts()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsAdmin = IsAdmin();
            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(User user)
        {
            
            AutoRSContext db = new AutoRSContext();
            user.IdRole = 3;

            // check for email duplicates

            User email = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            try
            {
                if (email == null)
                {
                    user.Password = HashPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("EditAccounts", "Admin");
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



        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(User user)
        {
            AutoRSContext db = new AutoRSContext();
            User u = db.Users.FirstOrDefault(x => x.Id == user.Id);
            if (user.Surname != null) u.Surname = user.Surname;
            if (user.Name != null) u.Name = user.Name;
            if (user.Email != null) u.Email = user.Email;
            if (user.Address != null) u.Address = user.Address;
            if (user.Birthday != null) u.Birthday = user.Birthday;
            Debug.WriteLine("Name: " + user.Name + " " + u.Name);
            db.SaveChanges();
            return RedirectToAction("EditAccounts", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount(User user)
        {
            AutoRSContext db = new AutoRSContext();
            db.Remove(user);
            Debug.WriteLine("User " + user.Name + " removed");
            db.SaveChanges();
            return RedirectToAction("EditAccounts", "Admin");
        }
    }
}
