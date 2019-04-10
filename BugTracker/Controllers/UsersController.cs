using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext DbContext;

        private UserManager<ApplicationUser> DefaultUserManager;

        public UsersController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserManager()
        {
            List<UserViewModel> users = DbContext.Users
                .Select(p => new UserViewModel
                {
                    Id = p.Id,
                    DisplayName = p.DisplayName,
                    Email = p.Email
                }).ToList();

            return View(users);
        }
    }
}