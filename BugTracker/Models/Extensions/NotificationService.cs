using Blog.Models.Extensions;
using BugTracker.Models.Classes;

namespace BugTracker.Models.Extensions
{
    public static class NotificationService
    {
        public static void Create(Ticket ticket, string data)
        {
            Notification notification = new Notification
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                Data = data
            };

            SendNotification(ticket.AssignedTo, notification);

            foreach (ApplicationUser user in ticket.Subscribers)
            {
                SendNotification(user, notification);
            }
        }

        public static void SendNotification(ApplicationUser user, Notification notification)
        {
            MyEmailService emailService = new MyEmailService();

            emailService.Send(user.Email,
                              "MyBugTracker Notification, Re " + notification.Ticket.Title + " in Project " + notification.Ticket.Project.Name,
                              notification.Data);
        }
    }
}