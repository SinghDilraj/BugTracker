using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class AssignProjectMembersViewModel
    {
        public int? Id { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public AssignProjectMembersViewModel()
        {
            Users = new List<ApplicationUser>();
        }
    }
}