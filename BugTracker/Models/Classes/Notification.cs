namespace BugTracker.Models.Classes
{
    public class Notification
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }
    }
}