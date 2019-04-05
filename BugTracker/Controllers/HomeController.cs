using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        private UserManager<ApplicationUser> DefaultUserManager;

        public HomeController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserManager()
        {
            List<ApplicationUser> users = DbContext.Users.Select(p => p).ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Roles(UserManagerRolesViewModel model, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                if (DefaultUserManager.IsInRole(id, "Admin"))
                {
                    model.Admin = true;
                }
                else
                {
                    model.Admin = false;
                }

                if (DefaultUserManager.IsInRole(id, "Project Manager"))
                {
                    model.ProjectManager = true;
                }
                else
                {
                    model.ProjectManager = false;
                }

                if (DefaultUserManager.IsInRole(id, "Developer"))
                {
                    model.Developer = true;
                }
                else
                {
                    model.Developer = false;
                }

                if (DefaultUserManager.IsInRole(id, "Submitter"))
                {
                    model.Submitter = true;
                }
                else
                {
                    model.Submitter = false;
                }

                model.Id = id;

                return PartialView("_Roles", model);
            }
        }

        [HttpPost]
        [ActionName("Roles")]
        [Authorize(Roles = "Admin")]
        public ActionResult RolesPost(UserManagerRolesViewModel model, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                if (model.Admin == true)
                {
                    DefaultUserManager.AddToRole(id, "Admin");
                }
                else
                {
                    DefaultUserManager.RemoveFromRole(id, "Admin");
                }

                if (model.ProjectManager == true)
                {
                    DefaultUserManager.AddToRole(id, "Project Manager");
                }
                else
                {
                    DefaultUserManager.RemoveFromRole(id, "Project Manager");
                }

                if (model.Developer == true)
                {
                    DefaultUserManager.AddToRole(id, "Developer");
                }
                else
                {
                    DefaultUserManager.RemoveFromRole(id, "Developer");
                }

                if (model.Submitter == true)
                {
                    DefaultUserManager.AddToRole(id, "Submitter");
                }
                else
                {
                    DefaultUserManager.RemoveFromRole(id, "Submitter");
                }

                return RedirectToAction(nameof(HomeController.UserManager));
            }
        }


        [Authorize(Roles = "Admin, Project Manager")]
        [HttpGet]
        public ActionResult CreateProject()
        {
            return View();
        }


        [Authorize(Roles = "Admin, Project Manager")]
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

            return RedirectToAction(nameof(HomeController.AllProjects));
        }

        [Authorize(Roles = "Admin, Project Manager")]
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

        [HttpGet]
        public ActionResult MyProjects()
        {
            string userId = User.Identity.GetUserId();
            List<HomeProjectViewModel> projects = DbContext.Projects.Where(p => p.Users.Any(x => x.Id == userId)).Select(p =>
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