using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext;

        public HomeController()
        {
            DbContext = new ApplicationDbContext();

        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult UserManager()
        {
            List<ApplicationUser> users = DbContext.Users.Select(p => p).ToList();
            return View(users);
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}