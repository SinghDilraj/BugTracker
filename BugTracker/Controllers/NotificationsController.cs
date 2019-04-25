using Blog.Models.Extensions;
using BugTracker.Controllers.HelperController;
using BugTracker.Models.Classes;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class NotificationsController : BaseController
    {
        // GET: Notification
        public void SendNotification(Notification notification)
        {
            MyEmailService emailService = new MyEmailService();

            emailService.Send(User.Identity.Name,
                              "MyBugTracker Notification, Re " + notification.Ticket.Title + " in Project " + notification.Ticket.Project.Name,
                              notification.Data);
        }
    }
}