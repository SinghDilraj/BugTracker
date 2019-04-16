using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}