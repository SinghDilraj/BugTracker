using BugTracker.Models.Classes;
using System.Data.Entity;

namespace BugTracker.Models.Extensions
{
    public static class HistoryService
    {
        public static History Create(ApplicationUser user, Ticket ticket, string prop, string oldValue, string newValue)
        {
            History history = new History
            {
                ChangingUserId = user.Id,
                ChangingUser = user,
                Ticket = ticket,
                TicketId = ticket.Id,
                PropertyChanged = prop,
                OldValue = oldValue,
                NewValue = newValue
            };

            return history;
        }
    }
}