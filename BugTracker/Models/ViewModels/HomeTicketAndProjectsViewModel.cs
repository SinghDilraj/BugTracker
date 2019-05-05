using BugTracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class HomeTicketAndProjectsViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}