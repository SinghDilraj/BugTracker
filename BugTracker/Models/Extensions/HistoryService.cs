using BugTracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Extensions
{
    public static class HistoryService
    {
        public static void Create(ApplicationUser user)
        {
            History history = new History
            {
                ChangingUserId = user.Id,
                ChangingUser = user
            };
        }
    }
}