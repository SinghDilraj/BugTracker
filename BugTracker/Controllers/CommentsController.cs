using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly UserManager<ApplicationUser> DefaultUserManager;
        private const string Submitter = "Submitter";
        private const string Admin = "Admin";
        private const string ProjectManager = "Project Manager";
        private const string Developer = "Developer";

        public CommentsController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        [HttpPost]
        public ActionResult AddComment(CommentViewModel model, int ticketId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            Comment comment = new Comment()
            {
                Title = model.Title,
                CreatedBy = DefaultUserManager.FindById(User.Identity.GetUserId()),
                Ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId)
            };

            ApplicationUser user = DefaultUserManager.FindById(User.Identity.GetUserId());

            Ticket ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId);

            if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
            {
                DbContext.Comments.Add(comment);

                DbContext.SaveChanges();

                return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
            }
            else if (User.IsInRole(Submitter))
            {
                if (ticket.CreatedBy == user)
                {
                    DbContext.Comments.Add(comment);

                    DbContext.SaveChanges();

                    return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
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
                    DbContext.Comments.Add(comment);

                    DbContext.SaveChanges();

                    return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.AllTickets));
                }
            }
            else
            {
                return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
            }
        }
    }
}