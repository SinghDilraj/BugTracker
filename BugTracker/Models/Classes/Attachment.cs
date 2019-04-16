using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Attachment
    {
        public int Id { get; set; }
        public HttpPostedFileBase File { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}