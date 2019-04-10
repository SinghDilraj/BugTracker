﻿using BugTracker.Models;
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
    public class RolesController : Controller
    {
        private ApplicationDbContext DbContext;

        private UserManager<ApplicationUser> DefaultUserManager;

        public RolesController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
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

                return RedirectToAction(nameof(UsersController.UserManager));
            }
        }
    }
}