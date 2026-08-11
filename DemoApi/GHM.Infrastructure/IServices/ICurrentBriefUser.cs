using GHM.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GHM.Infrastructure.IServices
{
    public interface ICurrentBriefUser
    {
        BriefUser CurrentLoginBriefUser(HttpContext context);
    }
}
