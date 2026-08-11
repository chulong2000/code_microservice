using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GHM.Infrastructure.IServices
{
    public interface IBaseHttpClientService
    {
        Task<JsonElement> GetAsync(string fullUrl, CancellationToken ct, int retryCount = 3,Dictionary<string, string> customHeaders = null);
        Task<JsonElement> PostAsync<T>(string fullUrl, T payload, CancellationToken ct, int retryCount = 3, Dictionary<string, string> customHeaders = null);

    }
}
