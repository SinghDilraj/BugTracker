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
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
        public virtual Project Project { get; set; }

        public Ticket()
        {
            DateCreated = DateTime.Now; 
        }
    }
}