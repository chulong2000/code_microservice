using GHM.Infrastructure.Extensions;
using GHM.Infrastructure.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GHM.Infrastructure.Models
{
    public class CurrentBriefUser : ControllerBase, ICurrentBriefUser
    {
        public BriefUser CurrentLoginBriefUser(HttpContext context)
        {
            if (context == null)
            {
                return new BriefUser
                {
                    Id = string.Empty,
                    FullName = string.Empty,
                    Avatar = string.Empty,
                    TenantId = string.Empty,
                    UserName = string.Empty
                };
            }
            return context.GetCurrentUser();
        }
    }
}
