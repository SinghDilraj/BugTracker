using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly UserManager<ApplicationUser> DefaultUserManager;

        public AttachmentsController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        [HttpPost]
        public ActionResult AddAttachment(AttachmentViewModel model)
        {
            Attachment attachment = new Attachment()
            {
                FileUrl = model.file.FileName,
                CreatedBy = DefaultUserManager.FindById(User.Identity.GetUserId())
            };

            return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
        }
    }
}