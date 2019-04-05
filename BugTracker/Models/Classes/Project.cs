using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<Ticket> Tickets { get; set; }

        public Project()
        {
            DateCreated = DateTime.Now;
            Tickets = new List<Ticket>();
        }
    }
}