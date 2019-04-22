using Blog.Models.Extensions;
using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly UserManager<ApplicationUser> DefaultUserManager;
        private const string Submitter = "Submitter";
        private const string Admin = "Admin";
        private const string ProjectManager = "Project Manager";
        private const string Developer = "Developer";

        public AttachmentsController()
        {
            DbContext = new ApplicationDbContext();

            DefaultUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        [HttpPost]
        public ActionResult AddAttachment(AttachmentViewModel model, int ticketId)
        {
            if (ModelState.IsValid)
            {
                if (!Directory.Exists(FileHelper.MappedUploadFolder))
                {
                    Directory.CreateDirectory(FileHelper.MappedUploadFolder);
                }

                string fileName = model.file.FileName;
                string fullPathWithName = FileHelper.MappedUploadFolder + fileName;

                model.file.SaveAs(fullPathWithName);

                Attachment attachment = new Attachment()
                {
                    FileUrl = FileHelper.UploadFolder + fileName,
                    FileName = fileName,
                    CreatedBy = DefaultUserManager.FindById(User.Identity.GetUserId()),
                    Ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId)
                };

                ApplicationUser user = DefaultUserManager.FindById(User.Identity.GetUserId());

                Ticket ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId);

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    DbContext.Attachments.Add(attachment);

                    DbContext.SaveChanges();

                    return RedirectToAction("Details", "Tickets", new { ticketId = model.Id });
                }
                else if (User.IsInRole(Submitter))
                {
                    if (ticket.CreatedBy == user)
                    {
                        DbContext.Attachments.Add(attachment);

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
                        DbContext.Attachments.Add(attachment);

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
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}