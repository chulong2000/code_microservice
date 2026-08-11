using GHM.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GHM.Infrastructure.Identity
{
    public static class ConfigureAuthAuthorHandler
    {
        public static void ConfigureAuthenticationHandler(this IServiceCollection services)
        {
            services.AddAuthentication(MyAuthenticationOptions.AuthenticationScheme)
             .AddScheme<MyAuthenticationOptions, MyAuthenticationHandler>(MyAuthenticationOptions.AuthenticationScheme, options => { });
        }

    }
}