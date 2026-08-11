using System;
using System.Threading.Tasks;
using GHM.Infrastructure.Constants;

namespace GHM.Infrastructure.IServices;

public interface ICacheFactory
{
    T GetOrSet<T>(string cacheKey, Func<T> factory, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute);
    Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute);
    void Get<T>(string cacheKey, out T value);
    void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute);
    void Remove(string cacheKey);
    void Clear();
}