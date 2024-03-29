﻿using Blog.Models.Extensions;
using BugTracker.Controllers.HelperController;
using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.Extensions;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class AttachmentsController : BaseController
    {
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

                Ticket ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId && !p.Project.Archived);

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    NotificationService.Create(ticket, $"Ticket attachment added ->> {attachment.FileName}");

                    DbContext.Attachments.Add(attachment);

                    DbContext.SaveChanges();

                    return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id });
                }
                else if (User.IsInRole(Submitter))
                {
                    if (ticket.CreatedBy == user)
                    {
                        NotificationService.Create(ticket, $"Ticket attachment added ->> {attachment.FileName}");

                        DbContext.Attachments.Add(attachment);

                        DbContext.SaveChanges();

                        return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id });
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
                        NotificationService.Create(ticket, $"Ticket attachment added ->> {attachment.FileName}");

                        DbContext.Attachments.Add(attachment);

                        DbContext.SaveChanges();

                        return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id });
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.AllTickets));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id });
                }
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id});
            }
        }

        public ActionResult DeleteAttachment(int? attachmentId)
        {
            if (attachmentId.HasValue)
            {
                Attachment attachment = DbContext.Attachments.FirstOrDefault(p => p.Id == attachmentId && !p.Ticket.Project.Archived);

                string userId = User.Identity.GetUserId();

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    DbContext.Attachments.Remove(attachment);

                    DbContext.SaveChanges();
                }
                else if (User.IsInRole(Submitter) || User.IsInRole(Developer))
                {
                    if (attachment.CreatedBy.Id == userId)
                    {
                        DbContext.Attachments.Remove(attachment);

                        DbContext.SaveChanges();
                    }
                }

                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = attachment.TicketId });
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets");
            }
        }
    }
}