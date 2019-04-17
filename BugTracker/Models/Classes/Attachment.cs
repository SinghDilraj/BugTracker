﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}