using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private const string AdminAndProjectManager = "Admin, Project Manager";
        private readonly ApplicationDbContext DbContext;

        public ProjectsController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpGet]
        public ActionResult CreateProject()
        {
            return View();
        }


        [Authorize(Roles = AdminAndProjectManager)]
        [HttpPost]
        public ActionResult CreateProject(HomeProjectViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Project project = new Project
            {
                Name = model.Name
            };

            DbContext.Projects.Add(project);

            DbContext.SaveChanges();

            return RedirectToAction(nameof(ProjectsController.AllProjects));
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpGet]
        public ActionResult AllProjects()
        {
            List<HomeProjectViewModel> projects = DbContext.Projects.Select(p =>
                new HomeProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated,
                    Users = p.Users,
                    Tickets = p.Tickets
                }).ToList();

            return View(projects);
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpGet]
        public ActionResult EditProject(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }

            HomeProjectViewModel project = DbContext.Projects
                .Where(p => p.Id == id)
                .Select(p => new HomeProjectViewModel
                {
                    Name = p.Name,
                }).ToList().FirstOrDefault();

            return View(project);
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpPost]
        public ActionResult EditProject(HomeProjectViewModel model, int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Project project = DbContext.Projects
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }

            project.Name = model.Name;
            project.DateUpdated = DateTime.Now;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(ProjectsController.AllProjects));
        }

        [Authorize(Roles = AdminAndProjectManager)]
        public ActionResult DeleteProject(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }

            Project project = DbContext.Projects
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }

            DbContext.Projects.Remove(project);

            DbContext.SaveChanges();

            return RedirectToAction(nameof(ProjectsController.AllProjects));
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpPost]
        public ActionResult AssignProject(int? projectId, string userId, bool add)
        {
            if (!projectId.HasValue || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(ProjectsController.AllProjects));
            }
            else
            {
                Project project = DbContext.Projects.
                    FirstOrDefault(p => p.Id == projectId);

                ApplicationUser user = DbContext.Users
                .FirstOrDefault(p => p.Id == userId);

                if (add)
                {
                    user.Projects.Add(project);
                }
                else
                {
                    user.Projects.Remove(project);
                }

                DbContext.SaveChanges();

                return RedirectToAction(nameof(UsersController.AllUsersForProjects), "Users");
            }
        }

        [HttpGet]
        public ActionResult MyProjects()
        {
            string userId = User.Identity.GetUserId();

            List<HomeProjectViewModel> projects = DbContext.Projects
                .Where(p => p.Users.Any(x => x.Id == userId))
                .Select(p =>
                new HomeProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated,
                    Users = p.Users,
                    Tickets = p.Tickets
                }).ToList();

            return View(projects);
        }
    }
}