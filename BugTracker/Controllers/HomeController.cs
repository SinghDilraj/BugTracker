﻿using BugTracker.Controllers.HelperController;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Models.ApplicationUser user = DefaultUserManager.FindByEmailAsync(User.Identity.Name).Result;

            HomeTicketAndProjectsViewModel model = new HomeTicketAndProjectsViewModel();

            if (User.IsInRole(Submitter))
            {
                model.Tickets = user.CreatedTickets;
                model.Projects = user.Projects;
            }
            else if (User.IsInRole(Developer))
            {
                model.Tickets = user.AssignedTickets;
                model.Projects = user.Projects;
            }
            else
            {
                model.Tickets = DbContext.Tickets.Select(p => p).ToList();
                model.Projects = DbContext.Projects.Select(p => p).ToList();
            }

            return View(model);
        }

        public PartialViewResult Layout()
        {
            Models.ApplicationUser user = DefaultUserManager.FindByEmailAsync(User.Identity.Name).Result;

            HomeTicketAndProjectsViewModel model = new HomeTicketAndProjectsViewModel();

            if (User.IsInRole(Submitter))
            {
                model.Tickets = user.CreatedTickets;
                model.Projects = user.Projects;
            }
            else if (User.IsInRole(Developer))
            {
                model.Tickets = user.AssignedTickets;
                model.Projects = user.Projects;
            }
            else
            {
                model.Tickets = DbContext.Tickets.Select(p => p).ToList();
                model.Projects = DbContext.Projects.Select(p => p).ToList();
            }

            if (User.IsInRole(Admin) || User.IsInRole(ProjectManager) || User.IsInRole(Developer))
            {
                model.Notifications = user.Notifications;
            }

            return PartialView("_LayoutTicketsAndProjects", model);
        }
    }
}