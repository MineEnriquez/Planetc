using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Planetc.Models;
using Microsoft.EntityFrameworkCore;    //MMGC: For entity handling
using Microsoft.AspNetCore.Identity;    //MMGC:  For password hashing.
using Microsoft.AspNetCore.Http;

namespace Planetc.Controllers
{
    public class HomeController : Controller
    {
        private PlanetcDBContext dbContext;
        public HomeController(PlanetcDBContext context) { dbContext = context; }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {


            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return View("LoginAndReg");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Index");
            // return View("Success");

        }

        #region Event Planner
        [Route("EventPlanner")]
        [HttpGet]
        public IActionResult EventPlannerGet()
        {
            ViewBag.Products = dbContext.Products.ToList();
             return View("EventPlanner");
        }
        #endregion
        #region Event Planner Layout
        [Route("EventPlannerLayout")]
        [HttpGet]
        public IActionResult EventPlannerLayoutGet()
        {
            ViewBag.Products = dbContext.Products.ToList();
             return View("EventPlannerLayout");
        }
        #endregion
        //============================================================================================//

        #region LOGIN AND REGISTRATION
        //-----------------

        [HttpPost("Register")]
        // public IActionResult Register(User _user)
        public IActionResult Register(ModelForLoginPage information)
        {
            User _user = information.Register;
            // Check initial ModelState
            if (ModelState.IsValid)
            {
                // If a User exists with provided email
                if (dbContext.Users.Any(u => u.Email == _user.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Register.Email", "Email already in use!");
                    return View("LoginAndReg");
                    // You may consider returning to the View at this point
                }
                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                _user.Password = Hasher.HashPassword(_user, _user.Password);

                dbContext.Add(_user);
                dbContext.SaveChanges();
                ViewBag.Email = _user.Email;
                return View("LoginAndReg");  //if registration is successfull, what? return to the first page and wait for the user to login?
                                             //or maybe go to the success page with the user already registered and logged in?
            }
            else
            {
                // Oh no!  We need to return a ViewResponse to preserve the ModelState, and the errors it now contains!
                return View("LoginAndReg");
            }
        }

        [Route("Login")]
        [HttpPost]
        // public IActionResult Login(LoginUser userSubmission)
        public IActionResult Login(ModelForLoginPage information)
        {
            LoginUser userSubmission = information.Login;
            HttpContext.Session.Clear();
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);

                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("LoginAndReg");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    //Clean up the session's user Id:
                    return View("LoginAndReg");

                }

                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    HttpContext.Session.SetString("UserName", userInDb.UserName);
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    HttpContext.Session.SetString("UserName", userInDb.UserName);
                }
                return Redirect("/");
            }
            else
            {
                // Oh no!  We need to return a ViewResponse to preserve the ModelState, and the errors it now contains!
                return View("LoginAndReg");
            }
        }

        public void CleanUpUserId()
        {
            HttpContext.Session.Clear();
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    
        #region Data
        [Route("Products")]
        public IEnumerable<Product> GetProducts()
        {
            return dbContext.Products.ToList();
        }
        #endregion

        [Route("AddWedding")]
        [HttpGet]
        public IActionResult DisplayTheAddWeddingPage()
        {
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View("AddWedding");
        }

        [Route("CreateWedding")]
        [HttpPost]
        public IActionResult CreateWedding(Wedding _newWedding)
        {
           if(ModelState.IsValid)
            {
                dbContext.Weddings.Add(_newWedding);
                dbContext.SaveChanges();

                return Redirect("/EventPlanner");
            }
            else
            {
                // Oh no!  We need to return a ViewResponse to preserve the ModelState, and the errors it now contains!
                return View("AddWedding");
            }
        }



        }
}
