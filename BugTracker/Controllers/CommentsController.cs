using BugTracker.Controllers.HelperController;
using BugTracker.Models;
using BugTracker.Models.Classes;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class CommentsController : BaseController
    {
        [HttpPost]
        public ActionResult AddComment(CommentViewModel model, int ticketId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId });
            }

            ApplicationUser user = DefaultUserManager.FindById(User.Identity.GetUserId());

            Ticket ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId);

            Comment comment = new Comment()
            {
                Title = model.Title,
                CreatedBy = user,
                Ticket = ticket
            };

            if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
            {
                DbContext.Comments.Add(comment);

                DbContext.SaveChanges();

                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = model.Id });
            }
            else if (User.IsInRole(Submitter))
            {
                if (ticket.CreatedBy == user)
                {
                    DbContext.Comments.Add(comment);

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
                    DbContext.Comments.Add(comment);

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

        public ActionResult DeleteComment(int? commentId)
        {
            if (commentId.HasValue)
            {
                Comment comment = DbContext.Comments.FirstOrDefault(p => p.Id == commentId);

                string userId = User.Identity.GetUserId();

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    DbContext.Comments.Remove(comment);

                    DbContext.SaveChanges();
                }
                else if (User.IsInRole(Submitter) || User.IsInRole(Developer))
                {
                    if (comment.CreatedBy.Id == userId)
                    {
                        DbContext.Comments.Remove(comment);

                        DbContext.SaveChanges();
                    }
                }

                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = comment.TicketId });
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets");
            }
        }

        [HttpGet]
        public ActionResult EditComment(int? commentId)
        {
            if (commentId.HasValue)
            {
                Comment comment = DbContext.Comments.FirstOrDefault(p => p.Id == commentId);

                string userId = User.Identity.GetUserId();

                CommentViewModel model = new CommentViewModel
                {
                    Title = comment.Title,
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    Ticket = comment.Ticket,
                    DateCreated = comment.Created
                };

                if (User.IsInRole(Admin) || User.IsInRole(ProjectManager))
                {
                    return View(model);
                }
                else if (User.IsInRole(Submitter) || User.IsInRole(Developer))
                {
                    if (comment.CreatedBy.Id == userId)
                    {
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = comment.TicketId });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = comment.TicketId });
                }
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets");
            }
        }

        [HttpPost]
        public ActionResult EditComment(int? commentId, CommentViewModel model)
        {
            if (commentId.HasValue)
            {
                Comment comment = DbContext.Comments.FirstOrDefault(p => p.Id == commentId);

                if (ModelState.IsValid)
                {
                    comment.Title = model.Title;

                    DbContext.SaveChanges();

                    return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = comment.TicketId });
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = comment.TicketId });
                }
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Details), "Tickets");
            }
        }
    }
}