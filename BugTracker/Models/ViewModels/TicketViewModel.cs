using BugTracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class TicketViewModel
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

        public int StatusId { get; set; }
        public int Id { get; set; }
        public TicketType Type { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ApplicationUser AssignedTo { get; set; }
        public Project Project { get; set; }
        public string TypeName { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string AssignedToName { get; set; }
        public string ProjectName { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<History> Histories { get; set; }
    }
}