using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }
    }
}