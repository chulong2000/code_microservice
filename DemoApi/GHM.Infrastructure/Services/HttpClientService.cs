using GHM.Infrastructure.Extensions;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GHM.Infrastructure.Services
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient Client { get; }
        private readonly ApiUrlSettings _apiUrls;
        private readonly ApiServiceInfo _apiServiceInfo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [Obsolete]
        public HttpClientService()
        {
            _httpContextAccessor = new HttpContextAccessor();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appSettingJsonFile = $"appsettings.{environment}.json";
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(appSettingJsonFile);
            var configuration = builder.Build();
            if (configuration != null)
            {
                _apiUrls = configuration.GetApiUrl();
                _apiServiceInfo = configuration.GetApiServiceInfo();
            }

            Client = Task.Run(GetClient).Result;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            #region Local Function.
            HttpClient GetClient()
            {
                var client = new HttpClient();

                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                //var access_Token = accessToken.Split(' ')[1];

                if (!String.IsNullOrEmpty(accessToken))
                {
                   client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken["Bearer ".Length..].Trim());
                    //client.SetBearerToken(access_Token);
                }
                return client;
            }
            #endregion
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            if (Client == null)
                return default;

            var response = await Client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> DeleteAsync<T>(string requestUri)
        {
            if (Client == null)
                return default;

            var response = await Client.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T>(string requestUri, object p)
        {
            if (Client == null)
                return default;

            var serializedContent = JsonConvert.SerializeObject(p);
            var buffer = Encoding.UTF8.GetBytes(serializedContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(requestUri, byteContent);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T>(string requestUri, Dictionary<string, string> paramters = null)
        {
            if (Client == null)
                return default;

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var encodedContent = new FormUrlEncodedContent(paramters);
            var response = await Client.PostAsync(requestUri, encodedContent);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PutAsync<T>(string requestUri, object p)
        {
            if (Client == null)
                return default;

            var serializedContent = JsonConvert.SerializeObject(p);
            var buffer = Encoding.UTF8.GetBytes(serializedContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PutAsync(requestUri, byteContent);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<T> PutAsync<T>(string requestUri, Dictionary<string, string> paramters)
        {
            if (Client == null)
                return default;

            var encodedContent = new FormUrlEncodedContent(paramters);
            var response = await Client.PutAsync(requestUri, encodedContent);
            response.EnsureSuccessStatusCode();
            return !response.IsSuccessStatusCode ? default : ParseResponse<T>(await response.Content.ReadAsStringAsync());
        }

        #region Private
        private static T ParseResponse<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion

        ~HttpClientService()
        {
            if (Client != null)
                Client.Dispose();
        }
    }
}
