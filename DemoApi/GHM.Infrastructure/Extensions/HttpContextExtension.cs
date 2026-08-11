using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GHM.Infrastructure.Extensions
{
    public static class HttpContextExtension
    {
        public static BriefUser GetCurrentUser(this HttpContext context)
        {
            var userId = GetUserId(context);
            if (string.IsNullOrEmpty(userId))
                return new BriefUser
                {
                    Id = string.Empty,
                    FullName = string.Empty,
                    Avatar = string.Empty,
                    TenantId = string.Empty,
                    UserName = string.Empty,
                };

            return new BriefUser
            {
                Id = GetUserId(context),
                FullName = GetUserFullname(context),
                Avatar = GetUserAvatar(context),
                TenantId = GetTenantId(context),
                UserName = string.Empty,
            };
        }


        public static string GetClientId(this HttpContext context)
        {
            var payloadObject = ParseAccessToken(context);
            return payloadObject == null ? string.Empty : (string)payloadObject.Where(x => x.Type == "client_id").Select(x => x.Value).SingleOrDefault();
        }

        public static string GetTenantId(this HttpContext context)
        {
            var payloadObject = ParseAccessToken(context);
            return payloadObject == null ? string.Empty : (string)payloadObject.Where(x => x.Type == "tenantId").Select(x => x.Value).SingleOrDefault();
        }

        public static string GetUserId(this HttpContext context)
        {
            var payloadObject = ParseAccessToken(context);
            return payloadObject == null ? string.Empty : (string)payloadObject.Where(x => x.Type == "user_id").Select(x => x.Value).SingleOrDefault();
        }

        private static string GetUserFullname(this HttpContext context)
        {
            var payloadObject = ParseAccessToken(context);
            return payloadObject == null ? string.Empty : (string)payloadObject.Where(x => x.Type == "fullName").Select(x => x.Value).SingleOrDefault();
        }

        private static string GetUserAvatar(this HttpContext context)
        {
            var payloadObject = ParseAccessToken(context);
            return payloadObject == null ? string.Empty : (string)payloadObject.Where(x => x.Type == "avatar").Select(x => x.Value).SingleOrDefault();
        }

        private static List<Claim> ParseAccessToken(HttpContext context)
        {
            if (context != null && (context.User.Identity is ClaimsIdentity identity))
            {
                var claims = identity.Claims.ToList();
                return claims;
            }
            return null;
        }
    }
}
