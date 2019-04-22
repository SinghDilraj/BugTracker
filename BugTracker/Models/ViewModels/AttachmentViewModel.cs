using BugTracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class AttachmentViewModel
    {
        public int Id { get; set; }
        [Required]
        public HttpPostedFileBase file { get; set; }
        public DateTime DateCreated { get; set; }
        public Ticket Ticket { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}