using System;

namespace BugTracker.Models.Classes
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}