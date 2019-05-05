using BugTracker.Controllers.HelperController;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class NotificationsController : BaseController
    {
        [HttpPost]
        public ActionResult Subscribe(string userId, int? ticketId, bool subscribe)
        {
            if (!string.IsNullOrEmpty(userId) && ticketId.HasValue)
            {
                Models.Classes.Ticket ticket = DbContext.Tickets.FirstOrDefault(p => p.Id == ticketId && !p.Project.Archived);

                Models.ApplicationUser user = DbContext.Users.FirstOrDefault(p => p.Id == userId);

                if (subscribe)
                {
                    ticket.Subscribers.Add(user);

                    DbContext.SaveChanges();
                }
                else
                {
                    ticket.Subscribers.Remove(user);

                    DbContext.SaveChanges();
                }

                return RedirectToAction(nameof(TicketsController.Details), "Tickets", new { ticketId = ticket.Id });
            }

            return RedirectToAction(nameof(TicketsController.Details), "Tickets");
        }
    }
}