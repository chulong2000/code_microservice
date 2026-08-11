using GHM.Infrastructure.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
namespace GHM.Infrastructure.Services
{
    public class BaseHttpClientService : IBaseHttpClientService
    {
        private readonly HttpClient _http;
        private readonly ILogger<BaseHttpClientService> _logger;

        public BaseHttpClientService(HttpClient http, ILogger<BaseHttpClientService> logger)
        {
            _http = http;
            _logger = logger;
        }

        /// <summary>
        /// GET request trả về JsonElement
        /// </summary>
        public async Task<JsonElement> GetAsync(string fullUrl, CancellationToken ct, int retryCount = 3, Dictionary<string, string> customHeaders = null)
        {
            return await SendAsync(() =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                if (customHeaders != null)
                {
                    foreach (var h in customHeaders)
                        request.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }
                return _http.SendAsync(request, ct);
            }, fullUrl, retryCount, ct);
        }

        /// <summary>
        /// POST request với payload trả về JsonElement
        /// </summary>
        public async Task<JsonElement> PostAsync<T>(string fullUrl, T payload, CancellationToken ct, int retryCount = 3, Dictionary<string, string> customHeaders = null)
        {
            var json = JsonSerializer.Serialize(payload);     
            return await SendAsync(() =>
            {
                // Tạo mới request + content mỗi lần
                var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                // Thêm custom headers nếu có
                if (customHeaders != null)
                {
                    foreach (var h in customHeaders)
                        request.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }

                return _http.SendAsync(request, ct);
            }, fullUrl, retryCount, ct);
        }


        private async Task<JsonElement> SendAsync(Func<Task<HttpResponseMessage>> action, string endpoint, int retryCount, CancellationToken ct)
        {
            for (int attempt = 1; attempt <= retryCount; attempt++)
            {
                try
                {
                    var response = await action();
                    var responseText = await response.Content.ReadAsStringAsync(ct);

                    if (response.IsSuccessStatusCode)
                        return JsonDocument.Parse(responseText).RootElement;

                    HandleHttpError(response, responseText);
                }
                catch (Exception ex) when (attempt < retryCount && IsRetryableError(ex))
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt - 1));
                    _logger?.LogWarning(ex, "Request {Endpoint} failed (attempt {Attempt}/{Total}). Retry in {Delay}s",
                        endpoint, attempt, retryCount, delay.TotalSeconds);
                    await Task.Delay(delay, ct);
                }
            }

            throw new Exception($"Request {endpoint} failed after {retryCount} attempts");
        }

        private void HandleHttpError(HttpResponseMessage response, string responseBody)
        {
            var errorMessage = $"HTTP {(int)response.StatusCode}";
            try
            {
                var errorDoc = JsonDocument.Parse(responseBody);
                if (errorDoc.RootElement.TryGetProperty("error", out var errorObj) &&
                    errorObj.TryGetProperty("message", out var msgProp))
                {
                    errorMessage += $": {msgProp.GetString()}";
                }
            }
            catch { }

            throw new HttpRequestException($"{errorMessage}. Response: {responseBody}");
        }

        private bool IsRetryableError(Exception ex) =>
            ex is TaskCanceledException or OperationCanceledException ||
            ex.Message.Contains("429") ||
            ex.Message.Contains("500") ||
            ex.Message.Contains("502") ||
            ex.Message.Contains("503") ||
            ex.Message.Contains("504") ||
            ex.Message.Contains("network", StringComparison.OrdinalIgnoreCase) ||
            ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase) ||
            ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase);
    }
}