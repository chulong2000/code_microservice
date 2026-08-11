using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GHM.Infrastructure
{
    public class GhmControllerBase : ControllerBase
    {
        public BriefUser CurrentUser
        {
            get
            {
                CurrentBriefUser _currentBriefUser = new();
                var currentUser = _currentBriefUser.CurrentLoginBriefUser(HttpContext);
                return currentUser;
            }
        }
    }
}
