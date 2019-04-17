using System;

namespace BugTracker.Models.Classes
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}