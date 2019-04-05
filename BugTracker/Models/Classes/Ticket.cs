using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Ticket
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        public Ticket()
        {
            DateCreated = DateTime.Now;
        }
    }
}