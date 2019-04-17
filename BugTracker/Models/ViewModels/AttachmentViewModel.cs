using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class AttachmentViewModel
    {
        public int Id { get; set; }
        public HttpPostedFileBase file { get; set; }
        public DateTime DateCreated { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}