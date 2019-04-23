using System.Collections.Generic;

namespace BugTracker.Models.Classes
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Ticket> Tickets { get; set; }
    }
}