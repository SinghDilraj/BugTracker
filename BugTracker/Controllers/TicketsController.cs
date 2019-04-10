using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext DbContext;

        private UserManager<ApplicationUser> DefaultUserManager;

        public TicketsController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
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

                return RedirectToAction(nameof(TicketsController.MyTickets));
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
                List<TicketViewModel> model = User.IsInRole("Submitter")
                    ? DbContext.Tickets
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
                    }).ToList()
                    : DbContext.Tickets
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
                return View(model);
            }
        }
    }
}