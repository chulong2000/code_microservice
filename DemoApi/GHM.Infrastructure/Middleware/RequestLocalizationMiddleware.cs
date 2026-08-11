using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GHM.Infrastructure.Middleware
{
    public class RequestLocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestLocalizationOptions _requestLocalizationOptions;
        private readonly ILogger<RequestLocalizationMiddleware> _logger;

        public RequestLocalizationMiddleware(RequestDelegate next,
            IOptions<RequestLocalizationOptions> requestLocalizationOptions,
            ILogger<RequestLocalizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _requestLocalizationOptions = requestLocalizationOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var supportedCultures = new[]
               {
                    new CultureInfo("vi-VN"),
                    new CultureInfo("en-US")

                };
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
            CultureInfo.CurrentUICulture = new CultureInfo("vi-VN");

            var browserCultures = context.Request.GetTypedHeaders().AcceptLanguage?.OrderByDescending(x => x.Quality ?? 1) // Quality defines priority from 0 to 1, where 1 is the highest.
                                                                             .Select(x => x.Value.ToString())
                                                                             .ToArray() ?? Array.Empty<string>();

            var providerCookieCulture = _requestLocalizationOptions.RequestCultureProviders.SingleOrDefault(x => x.GetType() == typeof(CookieRequestCultureProvider));
            var cookieName = ((CookieRequestCultureProvider)providerCookieCulture).CookieName;
            var cookieCulture = context.Request.Cookies[cookieName] ?? "there is no cookie";

            if (string.IsNullOrWhiteSpace(context.Request.Query["culture"]) || context.Request.Cookies[cookieName] == null)
            {
                var search = true;
                foreach (var itemBrowserCulture in browserCultures.ToArray())
                {
                    if (search)
                    {
                        foreach (var itemSupportedCulture in supportedCultures.ToArray())
                        {
                            if (itemSupportedCulture.ToString().Contains(itemBrowserCulture.ToString()))
                            {
                                _logger.LogTrace($"{itemBrowserCulture} equal supportedCulture: {itemSupportedCulture}");
                                var culture = new CultureInfo(itemSupportedCulture.ToString());
                                CultureInfo.CurrentCulture = culture;
                                CultureInfo.CurrentUICulture = culture;
                                search = false;
                                break;
                            }
                        }

                        if (itemBrowserCulture.Trim().Length > 1)
                        {
                            foreach (var itemSupportedCulture in supportedCultures.ToArray())
                            {

                                if (itemSupportedCulture.ToString().Contains(itemBrowserCulture.ToString().Substring(0, 2)))
                                {
                                    _logger.LogTrace($"browserCulture: {itemBrowserCulture} equal supportedCulture: {itemSupportedCulture}");
                                    var culture = new CultureInfo(itemSupportedCulture.ToString());
                                    CultureInfo.CurrentCulture = culture;
                                    CultureInfo.CurrentUICulture = culture;
                                    search = false;
                                    break;
                                }

                            }
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
