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
        private const string Admin = "Admin";
        private const string AdminAndProjectManager = "Admin, Project Manager";
        private const string Developer = "Developer";
        private readonly ApplicationDbContext DbContext;

        public UsersController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize(Roles = Admin)]
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

        [Authorize(Roles = AdminAndProjectManager)]
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

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpGet]
        public ActionResult AllUsersForTickets(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.AllTickets), "Tickets");
            }
            else
            {
                IdentityRole role = DbContext.Roles.FirstOrDefault(p => p.Name == Developer);

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