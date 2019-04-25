using System;

namespace BugTracker.Models.Classes
{
    public class History
    {
        public int Id { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string PropertyChanged { get; set; }
        public virtual ApplicationUser ChangingUser { get; set; }
        public string ChangingUserId { get; set; }
        public DateTime DateChanged { get; set; }

        public History()
        {
            DateChanged = DateTime.Now;
        }
    }
}