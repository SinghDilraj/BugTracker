using BugTracker.Controllers.HelperController;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Models.ApplicationUser user = DefaultUserManager.FindByEmailAsync(User.Identity.Name).Result;
            if (User.IsInRole(Submitter))
            {
                Session["UserTickets"] = user.CreatedTickets;
                Session["UserProjects"] = user.Projects;
            }
            else if (User.IsInRole(Developer))
            {
                Session["UserTickets"] = user.AssignedTickets;
                Session["UserProjects"] = user.Projects;
            }
            else
            {
                Session["UserTickets"] = DbContext.Tickets;
                Session["UserProjects"] = user.Projects;
            }

            return View();
        }
    }
}