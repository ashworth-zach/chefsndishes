using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity;
using chefdishes.Models;
using Microsoft.EntityFrameworkCore;
namespace chefdishes.Controllers
{
    public class HomeController : Controller
    {
        private AllContext dbContext;

        public HomeController(AllContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            List<Chef> AllChefs = dbContext.chef.ToList();
            ViewBag.AllChefs = AllChefs;
            return View();
        }
        [HttpGet("newchef")]
        public IActionResult newchefsplash()
        {
            return View("newChef");
        }
        [HttpPost("AddChef")]
        public IActionResult AddChef(Chef newChef)
        {
            if (ModelState.IsValid)
            {
                // take the userReg object and convert it to User, with a hashed pw
                Chef newerChef = new Chef
                {
                    firstname = newChef.firstname,
                    lastname = newChef.lastname,
                    dob = newChef.dob,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,

                };
                // save the new user with hashed pw
                dbContext.chef.Add(newerChef);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("newChef", newChef);
        }
        [HttpGet("dishes")]
        public IActionResult DishesSplash()
        {
                // List<Dish> dishesChef = dbContext.dish.Include(chef => chef.cook).ToList();
            var dishesChef = dbContext.dish.Include(x => x.cook).ToList();

            List<Dish> AllDishes = dbContext.dish.ToList();
            ViewBag.AllDishes = AllDishes;
            return View("Dishes");
        }
        [HttpGet("newdish")]
        public IActionResult NewDishSplash()
        {
            List<Chef> AllChefs = dbContext.chef.ToList();
            ViewBag.AllChefs = AllChefs;
            return View("newDish");
        }
        [HttpPost("AddDish")]
        public IActionResult AddDish(Dish newdish)
        {
            if (ModelState.IsValid)
            {
                Chef RetrievedChef = dbContext.chef.FirstOrDefault(x => x.Chefid == newdish.Chefid);
                newdish.cook = RetrievedChef;
                Console.WriteLine(newdish.cook.firstname+"========================================");
                dbContext.dish.Add(newdish);
                dbContext.SaveChanges();
                return RedirectToAction("DishesSplash");
            }
            return View("NewDish");
        }
    }
}