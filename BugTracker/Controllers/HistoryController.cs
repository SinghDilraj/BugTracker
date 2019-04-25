using BugTracker.Controllers.HelperController;
using BugTracker.Models.Classes;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HistoryController : BaseController
    {
        // GET: History
        public void Create()
        {
            History history = new History
            {
                ChangingUserId = User.Identity.GetUserId(),
                ChangingUser = DefaultUserManager.FindById(User.Identity.GetUserId())
            };
        }
    }
}