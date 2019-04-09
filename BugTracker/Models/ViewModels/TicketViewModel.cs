using BugTracker.Models;
using BugTracker.Models.Classes;
using System;

namespace BugTracker.Models.ViewModels
{
    public class TicketViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedByName { get; set; }
        public string AssignedToName { get; set; }
        public string ProjectName { get; set; }
    }
}