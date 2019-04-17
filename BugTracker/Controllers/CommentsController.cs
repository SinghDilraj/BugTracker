using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly UserManager<ApplicationUser> DefaultUserManager;

        public CommentsController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        [HttpPost]
        public ActionResult AddComment(CommentViewModel model)
        {
            Comment comment = new Comment()
            {
                Title = model.Title,
                CreatedBy = DefaultUserManager.FindById(User.Identity.GetUserId()),
            };

            DbContext.Comments.Add(comment);

            DbContext.SaveChanges();

            return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
        }

    }
}