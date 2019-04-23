using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime DateCreated { get; set; }

        public Attachment()
        {
            DateCreated = DateTime.Now;
        }
    }
}