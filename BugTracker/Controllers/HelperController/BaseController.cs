using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BugTracker.Controllers.HelperController
{
    public class BaseController : Controller
    {
        public const string Submitter = "Submitter";
        public const string Admin = "Admin";
        public const string ProjectManager = "Project Manager";
        public const string Developer = "Developer";
        public const string AdminAndProjectManagerAndSubmitter = "Admin, Project Manager, Submitter";
        public const string AdminAndProjectManager = "Admin, Project Manager";
        public const string SubmitterAndDeveloper = "Submitter, Developer";
        public readonly ApplicationDbContext DbContext;
        public readonly UserManager<ApplicationUser> DefaultUserManager;

        public BaseController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        public void Populate()
        {
            ApplicationUser user = DefaultUserManager.FindById(User.Identity.GetUserId());

        }
    }
}