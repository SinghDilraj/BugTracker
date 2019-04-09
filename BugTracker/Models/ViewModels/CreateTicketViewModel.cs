using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class CreateTicketViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        public string PriorityId { get; set; }
    }
}