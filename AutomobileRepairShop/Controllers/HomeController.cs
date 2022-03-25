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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AutoRSContext db = new AutoRSContext();

        // HOME RELATED METHODS
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // console checker for user login
            ViewBag.IsLogged = IsLogged();
            Debug.WriteLine(ViewBag.IsLogged.ToString() as string);
            ViewBag.IsAdmin = IsAdmin();
            Debug.WriteLine(ViewBag.IsAdmin.ToString() as string);
            return View();
        }

        //verifies is there is a user that is logged in
        public bool IsLogged()
        {
            bool isLogged = false;
            var loggedInUser = HttpContext.User;
            var claym = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if(claym != null)
                isLogged = true;
            return isLogged;
        }

        //get name of the logged user
        public string GetName()
        {
            return this.User.FindFirst(ClaimTypes.Name).Value;
        }

        public bool IsAdmin()
        {
            bool isAdmin = false;
            ClaimsPrincipal loggedInUser = HttpContext.User;
            // claym contains the role of the logged in user
            
                var claym = loggedInUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                if(claym != null)
                    if (claym.Value.ToString() == "Administrator")
                        isAdmin = true;

            return isAdmin;
        }

        public IActionResult Privacy()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult Welcome()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [HttpPost]
        public ActionResult Index(User obj)
        {
            return View(obj);
        }

        [Authorize (Roles="Administrator")]
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

        

        // LOGIN RELATED METHODS
        public IActionResult Login()
        {
            ViewBag.IsLogged = IsLogged();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(User user)
        {
            AutoRSContext db = new AutoRSContext();
            User user1 = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            if (user1 == null)
            {
                ViewBag.EmailNotExists = "Account doesn't exists!";
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
            {
                Debug.WriteLine(ex.ToString());
            }
            return View();

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
                    Debug.WriteLine("coaiele mele");
                    user.Password = HashPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("EditAccounts", "Home");
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

        // ADMIN PANEL RELATED METHODS
        
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
            return RedirectToAction("EditAccounts", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount(User user)
        {
            AutoRSContext db = new AutoRSContext();
            db.Remove(user);
            Debug.WriteLine("User " + user.Name + " removed");
            db.SaveChanges();
            return RedirectToAction("EditAccounts", "Home");
        }

        public ActionResult Bills()
        {
            return View();
        }

        public ActionResult Appointments()
        {
            return View();
        }
    }
}
