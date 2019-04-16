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

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpGet]
        public ActionResult AllUsersForProjects(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects), "Projects");
            }
            else
            {
                AssignProjectMembersViewModel model = new AssignProjectMembersViewModel
                {
                    Id = id,
                    Users = DbContext.Users.ToList()
                };

                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpGet]
        public ActionResult AllUsersForTickets(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.AllTickets), "Tickets");
            }
            else
            {
                IdentityRole role = DbContext.Roles.FirstOrDefault(p => p.Name == "Developers");

                AssignProjectMembersViewModel model = new AssignProjectMembersViewModel
                {
                    Id = id,
                    Users = DbContext.Users.Where(p => p.Roles.Any(q => q.RoleId == role.Id)).ToList()
                };

                return View(model);
            }
        }
    }
}