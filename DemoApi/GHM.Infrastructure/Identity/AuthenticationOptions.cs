using Microsoft.AspNetCore.Authentication;

namespace GHM.Infrastructure.Identity
{
    public class MyAuthenticationOptions :
        AuthenticationSchemeOptions
    {
        public const string AuthenticationScheme = "Bearer";
    }
}