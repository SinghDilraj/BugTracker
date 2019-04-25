using Blog.Models.Extensions;
using BugTracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Extensions
{
    public static class NotificationService
    {
        public static void SendNotification(ApplicationUser user, Notification notification)
        {
            MyEmailService emailService = new MyEmailService();

            emailService.Send(user.Email,
                              "MyBugTracker Notification, Re " + notification.Ticket.Title + " in Project " + notification.Ticket.Project.Name,
                              notification.Data);
        }
    }
}