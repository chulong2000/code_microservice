using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHM.Infrastructure.IServices
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string requestUri);
        Task<T> DeleteAsync<T>(string requestUri);
        Task<T> PostAsync<T>(string requestUri, object p);
        Task<T> PostAsync<T>(string requestUri, Dictionary<string, string> paramters);
        Task<T> PutAsync<T>(string requestUri, object p);
        Task<T> PutAsync<T>(string requestUri, Dictionary<string, string> paramters);

    }
}
