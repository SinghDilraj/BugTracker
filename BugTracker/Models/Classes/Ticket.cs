using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual TicketType Type { get; set; }
        public int TypeId { get; set; }
        public virtual TicketPriority Priority { get; set; }
        public int PriorityId { get; set; }
        public virtual TicketStatus Status { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
        public virtual List<History> Histories { get; set; }

        public Ticket()
        {
            DateCreated = DateTime.Now; 
        }
    }
}