using GHM.Infrastructure.Helpers;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GHM.Infrastructure.Identity

{
    public class MyAuthenticationHandler : AuthenticationHandler<MyAuthenticationOptions>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        [Obsolete]
        public MyAuthenticationHandler(HttpClient httpClient, IConfiguration configuration, IOptionsMonitor<MyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get the token from the Authorization header
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                return AuthenticateResult.Fail("Authorization header not found.");
            }

            var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Bearer token not found in Authorization header.");
            }

            //check client id
            if (!Context.Request.Headers.TryGetValue("TenantId", out var tenantIdHeaderValues))
            {
                return AuthenticateResult.Fail("ClientId header not found.");
            }

            var tenantId = tenantIdHeaderValues.FirstOrDefault();

            if (string.IsNullOrEmpty(tenantId))
            {
                return AuthenticateResult.Fail("TenantId not found in ClientId header.");
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();

            //Add Basic Authorization
            var authenticationString = $"{_configuration["ApiServiceInfo:ClientId"]}:{_configuration["ApiServiceInfo:ClientSecret"]}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            // Serialize class into JSON
            var body = new AuthTokenBody { token = token };

            // Wrap our JSON inside a StringContent object
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            // Bắt đầu đo thời gian
            var sw = Stopwatch.StartNew();
            // Call the API to validate the token
            var response = await _httpClient.PostAsync(_configuration["ApiServiceInfo:Url"], content);
            sw.Stop();

            // Log thông tin
            if (sw.ElapsedMilliseconds > 500) // >500ms coi là chậm
            {
                Logger.LogWarning(
                         "API call to {Url} took {Elapsed} ms (SLOW). StatusCode: {StatusCode}",
                         _configuration["ApiServiceInfo:Url"], sw.ElapsedMilliseconds, response.StatusCode);
            }

            // Return an authentication failure if the response is not successful
            if (!response.IsSuccessStatusCode)
            {
                return AuthenticateResult.Fail("Token validation failed.");
            }

            // Deserialize the response body to a custom object to get the validation result
            var validationResult = JsonConvert.DeserializeObject<AuthTokenResponse>(await response.Content.ReadAsStringAsync());

            // Return an authentication failure if the token is not valid
            if (!validationResult.Active)
            {
                return AuthenticateResult.Fail("Token is not valid.");
            }

            //Success! Add details here that identifies the user
            var claims = new List<Claim>()
            {
                new("tenantId", tenantId),
                new("user_id", validationResult.User_id),
                new("fullName", validationResult.Fullname),
                new("avatar", validationResult.Avatar ?? ""),
                new("client_id", validationResult.Client_id)
            };

            var claimsIdentity = new ClaimsIdentity
                 (claims, this.Scheme.Name);

            var claimsPrincipal = new ClaimsPrincipal
                (claimsIdentity);

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, this.Scheme.Name));
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Response.Headers.ContainsKey("www-authenticate"))
            {
                // Response.Headers.Append("access-control-allow-origin", "https://local.ttmedic.vn");
                Response.Headers.Append("access-control-expose-headers", "www-authenticate");
                Response.Headers.Append("www-authenticate",
                    "Bearer realm=\"HR\",error_description=\"The access token provided is expired, revoked, malformed, or invalid for other reasons.\",error=\"invalid_token\"");
                
            }

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
    }
}
