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
                model.Admin = DefaultUserManager.IsInRole(id, "Admin") ? true : false;

                model.ProjectManager = DefaultUserManager.IsInRole(id, "Project Manager") ? true : false;

                model.Developer = DefaultUserManager.IsInRole(id, "Developer") ? true : false;

                model.Submitter = DefaultUserManager.IsInRole(id, "Submitter") ? true : false;

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

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpGet]
        public ActionResult EditProject(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }

            HomeProjectViewModel project = DbContext.Projects
                .Where(p => p.Id == id)
                .Select(p => new HomeProjectViewModel
                {
                    Name = p.Name,
                }).ToList().FirstOrDefault();

            return View(project);
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        public ActionResult EditProject(HomeProjectViewModel model, int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Project project = DbContext.Projects
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }

            project.Name = model.Name;
            project.DateUpdated = DateTime.Now;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(HomeController.AllProjects));
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult DeleteProject(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }

            Project project = DbContext.Projects
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }

            DbContext.Projects.Remove(project);

            DbContext.SaveChanges();

            return RedirectToAction(nameof(HomeController.AllProjects));
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpGet]
        public ActionResult AllUsers(AssignProjectMembersViewModel model, int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
            }
            else
            {
                model.Id = id;
                model.Users = DbContext.Users.ToList();

                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        public ActionResult AssignProject(int? projectId, string userId, bool add)
        {
            if (!projectId.HasValue || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.AllProjects));
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

                return RedirectToAction(nameof(HomeController.AllUsers));
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

        [Authorize(Roles = "Submitter")]
        [HttpGet]
        public ActionResult CreateTicket()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                CreateTicketViewModel model = new CreateTicketViewModel();

                List<Project> projects = DbContext.Projects
                    .Where(p => p.Users.Any(q => q.Id == userId))
                    .Select(p => p)
                    .ToList();

                List<SelectListItem> projectList = new List<SelectListItem>();

                foreach (Project project in projects)
                {
                    SelectListItem item = new SelectListItem
                    {
                        Text = project.Name,
                        Value = project.Id.ToString()
                    };

                    projectList.Add(item);
                }

                ViewData["projects"] = projectList;

                List<SelectListItem> typeList = new List<SelectListItem>();

                foreach (TicketType type in DbContext.TicketTypes.Select(p => p))
                {
                    SelectListItem item = new SelectListItem
                    {
                        Text = type.Name,
                        Value = type.Id.ToString()
                    };

                    typeList.Add(item);
                }

                ViewData["types"] = typeList;

                List<SelectListItem> priorityList = new List<SelectListItem>();

                foreach (TicketPriority priority in DbContext.TicketPriorities.Select(p => p))
                {
                    SelectListItem item = new SelectListItem
                    {
                        Text = priority.Name,
                        Value = priority.Id.ToString()
                    };

                    priorityList.Add(item);
                }

                ViewData["priorities"] = priorityList;

                return View(model);
            }
        }

        [Authorize(Roles = "Submitter")]
        [HttpPost]
        public ActionResult CreateTicket(CreateTicketViewModel model)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                int projectId = Convert.ToInt32(model.ProjectId);
                int priorityId = Convert.ToInt32(model.PriorityId);
                int typeId = Convert.ToInt32(model.TypeId);

                Ticket ticket = new Ticket()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Project = DbContext.Projects.Where(p => p.Id == projectId).FirstOrDefault(),
                    Priority = DbContext.TicketPriorities.Where(p => p.Id == priorityId).FirstOrDefault(),
                    Type = DbContext.TicketTypes.Where(p => p.Id == typeId).FirstOrDefault(),
                    Status = DbContext.TicketStatuses.Where(p => p.Name == "Open").FirstOrDefault(),
                    CreatedBy = DefaultUserManager.FindById(User.Identity.GetUserId())
                };

                DbContext.Tickets.Add(ticket);

                DbContext.SaveChanges();

                return RedirectToAction(nameof(HomeController.MyTickets));
            }
        }

        [HttpGet]
        public ActionResult AllTickets()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                List<TicketViewModel> model = User.IsInRole("Submitter") || User.IsInRole("Developer")
                    ? DbContext.Tickets
                            .Where(p => p.Project.Users.Any(q => q.Id == userId))
                            .Select(p => new TicketViewModel
                            {
                                Title = p.Title,
                                Description = p.Description,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                ProjectName = p.Project.Name,
                                Type = p.Type.Name,
                                Priority = p.Priority.Name,
                                Status = p.Status.Name,
                                CreatedByName = p.CreatedBy.DisplayName,
                                AssignedToName = p.AssignedTo.DisplayName
                            }).ToList()
                    : DbContext.Tickets
                            .Select(p => new TicketViewModel
                            {
                                Title = p.Title,
                                Description = p.Description,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                ProjectName = p.Project.Name,
                                Type = p.Type.Name,
                                Priority = p.Priority.Name,
                                Status = p.Status.Name,
                                CreatedByName = p.CreatedBy.DisplayName,
                                AssignedToName = p.AssignedTo.DisplayName
                            }).ToList();
                return View(model);
            }
        }

        [Authorize(Roles = "Submitter, Developer")]
        [HttpGet]
        public ActionResult MyTickets()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                List<TicketViewModel> model;

                if (User.IsInRole("Submitter"))
                {
                    model = DbContext.Tickets
                    .Where(p => p.CreatedBy.Id == userId)
                    .Select(p => new TicketViewModel
                    {
                        Title = p.Title,
                        Description = p.Description,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        ProjectName = p.Project.Name,
                        Type = p.Type.Name,
                        Priority = p.Priority.Name,
                        Status = p.Status.Name,
                        CreatedByName = p.CreatedBy.DisplayName,
                        AssignedToName = p.AssignedTo.DisplayName
                    }).ToList();
                }
                else
                {
                    model = DbContext.Tickets
                    .Where(p => p.AssignedTo.Id == userId)
                    .Select(p => new TicketViewModel
                    {
                        Title = p.Title,
                        Description = p.Description,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        ProjectName = p.Project.Name,
                        Type = p.Type.Name,
                        Priority = p.Priority.Name,
                        Status = p.Status.Name,
                        CreatedByName = p.CreatedBy.DisplayName,
                        AssignedToName = p.AssignedTo.DisplayName
                    }).ToList();
                }
                
                return View(model);
            }
        }
    }
}