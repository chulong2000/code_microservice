using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace GHM.Infrastructure.Services;

public class CacheFactory : ICacheFactory
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);

    private readonly IMemoryCache _memoryCache;
    private readonly ConcurrentDictionary<string, byte> _trackedKeys = new();
    private readonly ConcurrentDictionary<string, Lazy<Task<object>>> _inFlight = new();

    public CacheFactory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public T GetOrSet<T>(string cacheKey, Func<T> factory, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute)
    {
        ValidateInputs(cacheKey, factory);

        if (_memoryCache.TryGetValue(cacheKey, out T cachedValue))
        {
            return cachedValue;
        }

        var value = factory();
        Set(cacheKey, value, absoluteExpirationRelativeToNow, expirationMode);
        return value;
    }

    public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute)
    {
        ValidateInputs(cacheKey, factory);

        if (_memoryCache.TryGetValue(cacheKey, out T cachedValue))
        {
            return cachedValue;
        }

        var inFlight = _inFlight.GetOrAdd(
            cacheKey,
            key => new Lazy<Task<object>>(
                async () =>
                {
                    try
                    {
                        if (_memoryCache.TryGetValue(key, out T existingValue))
                        {
                            return existingValue;
                        }

                        var value = await factory().ConfigureAwait(false);
                        Set(key, value, absoluteExpirationRelativeToNow, expirationMode);
                        return value;
                    }
                    finally
                    {
                        if (_inFlight.TryRemove(key, out var removedLazy))
                        {
                            GC.KeepAlive(removedLazy);
                        }
                    }
                },
                LazyThreadSafetyMode.ExecutionAndPublication));

        try
        {
            return (T)await inFlight.Value.ConfigureAwait(false)!;
        }
        catch
        {
            if (_inFlight.TryRemove(cacheKey, out var removedLazy))
            {
                GC.KeepAlive(removedLazy);
            }
            throw;
        }
    }

    public void Get<T>(string cacheKey, out T value)
    {
        ValidateCacheKey(cacheKey);

        if (_memoryCache.TryGetValue(cacheKey, out value))
        {
            return;
        }

        value = default;
    }

    public void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CacheExpirationMode expirationMode = CacheExpirationMode.Absolute)
    {
        ValidateCacheKey(cacheKey);

        var expiration = absoluteExpirationRelativeToNow ?? DefaultExpiration;
        EnsureValidExpiration(expiration);

        var options = CreateEntryOptions(expiration, expirationMode);
        _memoryCache.Set(cacheKey, value, options);
        _trackedKeys.TryAdd(cacheKey, 0);
    }

    public void Remove(string cacheKey)
    {
        ValidateCacheKey(cacheKey);

        _memoryCache.Remove(cacheKey);
        _trackedKeys.TryRemove(cacheKey, out _);
        if (_inFlight.TryRemove(cacheKey, out var removedLazy))
        {
            GC.KeepAlive(removedLazy);
        }
    }

    public void Clear()
    {
        foreach (var cacheKey in _trackedKeys.Keys.ToArray())
        {
            _memoryCache.Remove(cacheKey);
            _trackedKeys.TryRemove(cacheKey, out _);
            if (_inFlight.TryRemove(cacheKey, out var removedLazy))
            {
                GC.KeepAlive(removedLazy);
            }
        }
    }

    private static MemoryCacheEntryOptions CreateEntryOptions(TimeSpan expiration, CacheExpirationMode expirationMode)
    {
        return expirationMode switch
        {
            CacheExpirationMode.Sliding => new MemoryCacheEntryOptions
            {
                SlidingExpiration = expiration
            },
            CacheExpirationMode.Absolute => new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            },
            _ => throw new ArgumentOutOfRangeException(nameof(expirationMode), expirationMode, "Unsupported cache expiration mode.")
        };
    }

    private static void EnsureValidExpiration(TimeSpan expiration)
    {
        if (expiration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(expiration), "Cache expiration must be greater than zero.");
        }
    }

    private static void ValidateInputs<T>(string cacheKey, T factory)
    {
        ValidateCacheKey(cacheKey);
        ArgumentNullException.ThrowIfNull(factory);
    }

    private static void ValidateCacheKey(string cacheKey)
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new ArgumentException("Cache key cannot be null, empty, or whitespace.", nameof(cacheKey));
        }
    }
}

