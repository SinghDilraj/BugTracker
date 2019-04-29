﻿using BugTracker.Controllers.HelperController;
using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.Extensions;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : BaseController
    {
        [Authorize(Roles = Submitter)]
        [HttpGet]
        public ActionResult CreateTicket()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                TicketViewModel model = new TicketViewModel();

                List<Project> projects = DbContext.Projects
                    .Where(p => p.Users.Any(q => q.Id == userId))
                    .Select(p => p)
                    .ToList();

                SelectList projectList = new SelectList(projects, "Name", "Id");

                ViewData["projects"] = projectList;

                SelectList typeList = new SelectList(DbContext.TicketTypes, "Name", "Id");

                ViewData["types"] = typeList;

                SelectList priorityList = new SelectList(DbContext.TicketPriorities, "Name", "Id");

                ViewData["priorities"] = priorityList;

                return View(model);
            }
        }

        [Authorize(Roles = Submitter)]
        [HttpPost]
        public ActionResult CreateTicket(TicketViewModel model)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return View(model);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    List<Project> projects = DbContext.Projects
                        .Where(p => p.Users.Any(q => q.Id == userId))
                        .Select(p => p)
                        .ToList();

                    SelectList projectList = new SelectList(projects, "Name", "Id");

                    ViewData["projects"] = projectList;

                    SelectList typeList = new SelectList(DbContext.TicketTypes, "Name", "Id");

                    ViewData["types"] = typeList;

                    SelectList priorityList = new SelectList(DbContext.TicketPriorities, "Name", "Id");

                    ViewData["priorities"] = priorityList;

                    return View(model);
                }

                int projectId = model.ProjectId;
                int priorityId = Convert.ToInt32(model.PriorityId);
                int typeId = model.TypeId;

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
        public ActionResult EditTicket(int? ticketId)
        {
            string userId = User.Identity.GetUserId();

            if (!ticketId.HasValue && string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(TicketsController.AllTickets));
            }
            else
            {
                Ticket ticket = DbContext.Tickets
                    .FirstOrDefault(p => p.Id == ticketId);

                TicketViewModel model = new TicketViewModel
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Project = ticket.Project,
                    Type = ticket.Type,
                    Priority = ticket.Priority,
                    Status = ticket.Status,
                    AssignedTo = ticket.AssignedTo,
                };

                List<Project> projects = DbContext.Projects
                        .Where(p => p.Users.Any(q => q.Id == userId))
                        .Select(p => p)
                        .ToList();

                SelectList projectList = new SelectList(projects, "Name", "Id");

                ViewData["projects"] = projectList;

                SelectList typeList = new SelectList(DbContext.TicketTypes, "Name", "Id");

                ViewData["types"] = typeList;

                SelectList priorityList = new SelectList(DbContext.TicketPriorities, "Name", "Id");

                ViewData["priorities"] = priorityList;

                SelectList statusList = new SelectList(DbContext.TicketStatuses, "Name", "Id");

                ViewData["statuses"] = statusList;

                ApplicationUser user = DefaultUserManager.FindById(userId);

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    return View(model);
                }
                else if (User.IsInRole(Submitter))
                {
                    if (ticket.CreatedBy == user)
                    {
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                }
                else if (User.IsInRole(Developer))
                {
                    if (ticket.AssignedTo == user)
                    {
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.AllTickets));
                }
            }
        }

        [HttpPost]
        public ActionResult EditTicket(TicketViewModel model, int? ticketId)
        {
            string userId = User.Identity.GetUserId();

            if (!ModelState.IsValid || !ticketId.HasValue)
            {
                List<Project> projects = DbContext.Projects
                        .Where(p => p.Users.Any(q => q.Id == userId))
                        .Select(p => p)
                        .ToList();

                SelectList projectList = new SelectList(projects, "Name", "Id");

                ViewData["projects"] = projectList;

                SelectList typeList = new SelectList(DbContext.TicketTypes, "Name", "Id");

                ViewData["types"] = typeList;

                SelectList priorityList = new SelectList(DbContext.TicketPriorities, "Name", "Id");

                ViewData["priorities"] = priorityList;

                SelectList statusList = new SelectList(DbContext.TicketStatuses, "Name", "Id");

                ViewData["statuses"] = statusList;

                return View(model);
            }
            else
            {
                Ticket ticket = DbContext.Tickets
                    .FirstOrDefault(p => p.Id == ticketId);

                if (ticket.Title != model.Title)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Title", ticket.Title, model.Title);
                    ticket.Title = model.Title;
                };

                if (ticket.Description != model.Description)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Description", ticket.Description, model.Description);
                    ticket.Description = model.Description;
                };

                Project project = DbContext.Projects.FirstOrDefault(p => p.Id == model.ProjectId);

                if (ticket.Project != project)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Project", ticket.Project.Name, project.Name);
                    ticket.Project = project;
                };

                TicketType type = DbContext.TicketTypes.FirstOrDefault(p => p.Id == model.TypeId);

                if (ticket.Type != type)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Type", ticket.Type.Name, type.Name);
                    ticket.Type = type;
                };

                TicketPriority priority = DbContext.TicketPriorities.FirstOrDefault(p => p.Id == Convert.ToInt32(model.PriorityId));

                if (ticket.Priority != priority)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Priority", ticket.Priority.Name, priority.Name);
                    ticket.Priority = priority;
                };

                TicketStatus status = DbContext.TicketStatuses.FirstOrDefault(p => p.Id == model.StatusId);

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    if (ticket.Status != status)
                    {
                        HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Status", ticket.Status.Name, status.Name);
                        ticket.Status = status;
                    };
                }
                else
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Status", ticket.Status.Name, "Open");
                    ticket.Status = DbContext.TicketStatuses.FirstOrDefault(p => p.Name == "Open");
                };

                ticket.DateUpdated = DateTime.Now;

                DbContext.SaveChanges();

                return RedirectToAction(nameof(TicketsController.AllTickets));
            }
        }

        [Authorize(Roles = AdminAndProjectManagerAndSubmitter)]
        public ActionResult DeleteTicket(int? ticketId)
        {
            if (!ticketId.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.AllTickets));
            }
            else
            {
                string userId = User.Identity.GetUserId();

                ApplicationUser user = DefaultUserManager.FindById(userId);

                Ticket ticket = DbContext.Tickets
                    .FirstOrDefault(p => p.Id == ticketId);

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    DbContext.Tickets.Remove(ticket);

                    DbContext.SaveChanges();

                    return RedirectToAction(nameof(TicketsController.AllTickets));
                }
                else if (User.IsInRole(Submitter))
                {
                    if (ticket.CreatedBy == user)
                    {
                        DbContext.Tickets.Remove(ticket);

                        DbContext.SaveChanges();

                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                }
                else if (User.IsInRole(Developer))
                {
                    if (ticket.AssignedTo == user)
                    {
                        DbContext.Tickets.Remove(ticket);

                        DbContext.SaveChanges();

                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.AllTickets));
                }
            }
        }

        [Authorize(Roles = AdminAndProjectManager)]
        [HttpPost]
        public ActionResult AssignTicket(int? ticketId, string userId, bool add)
        {
            if (!ticketId.HasValue || string.IsNullOrEmpty(userId) || !ModelState.IsValid)
            {
                return RedirectToAction(nameof(TicketsController.AllTickets));
            }
            else
            {
                Ticket ticket = DbContext.Tickets
                    .FirstOrDefault(p => p.Id == ticketId);

                ApplicationUser user = DbContext.Users
                .FirstOrDefault(p => p.Id == userId);

                if (add)
                {
                    HistoryService.Create(DefaultUserManager.FindById(User.Identity.GetUserId()), ticket, "Assigned", ticket.AssignedTo.Email, user.Email);

                    ticket.AssignedTo = user;
                }
                else
                {
                    ticket.AssignedTo = null;
                }

                DbContext.SaveChanges();

                return RedirectToAction(nameof(UsersController.AllUsersForTickets), "Users");
            }
        }

        [HttpGet]
        public ActionResult Details(int? ticketId)
        {
            if (!ticketId.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.AllTickets), "Tickets");
            }
            else
            {
                TicketViewModel model = DbContext.Tickets
                    .Where(p => p.Id == ticketId)
                    .Select(p => new TicketViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        CreatedByName = p.CreatedBy.Email,
                        AssignedToName = p.AssignedTo.Email,
                        TypeName = p.Type.Name,
                        PriorityName = p.Priority.Name,
                        StatusName = p.Status.Name,
                        ProjectName = p.Project.Name,
                        Comments = p.Comments,
                        Attachments = p.Attachments,
                        Histories = p.Histories
                    })
                    .FirstOrDefault();

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult AllTickets()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                List<TicketViewModel> model = (User.IsInRole(Submitter)
                                               && !User.IsInRole(Admin)
                                               && !User.IsInRole(ProjectManager)) || (User.IsInRole(Developer) && !User.IsInRole(Admin) && !User.IsInRole(ProjectManager))
                    ? DbContext.Tickets
                            .Where(p => p.Project.Users.Any(q => q.Id == userId))
                            .Select(p => new TicketViewModel
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                ProjectName = p.Project.Name,
                                TypeName = p.Type.Name,
                                PriorityName = p.Priority.Name,
                                StatusName = p.Status.Name,
                                CreatedByName = p.CreatedBy.Email,
                                AssignedToName = p.AssignedTo.Email
                            }).ToList()
                    : DbContext.Tickets
                            .Select(p => new TicketViewModel
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                ProjectName = p.Project.Name,
                                TypeName = p.Type.Name,
                                PriorityName = p.Priority.Name,
                                StatusName = p.Status.Name,
                                CreatedByName = p.CreatedBy.Email,
                                AssignedToName = p.AssignedTo.Email
                            }).ToList();
                return View(model);
            }
        }

        [Authorize(Roles = SubmitterAndDeveloper)]
        [HttpGet]
        public ActionResult MyTickets()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                List<TicketViewModel> model = User.IsInRole(Submitter)
                    ? DbContext.Tickets
                    .Where(p => p.CreatedBy.Id == userId)
                    .Select(p => new TicketViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        ProjectName = p.Project.Name,
                        TypeName = p.Type.Name,
                        PriorityName = p.Priority.Name,
                        StatusName = p.Status.Name,
                        CreatedByName = p.CreatedBy.Email,
                        AssignedToName = p.AssignedTo.Email
                    }).ToList()
                    : DbContext.Tickets
                    .Where(p => p.AssignedTo.Id == userId)
                    .Select(p => new TicketViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        ProjectName = p.Project.Name,
                        TypeName = p.Type.Name,
                        PriorityName = p.Priority.Name,
                        StatusName = p.Status.Name,
                        CreatedByName = p.CreatedBy.Email,
                        AssignedToName = p.AssignedTo.Email
                    }).ToList();
                return View(model);
            }
        }
    }
}