using Convex.Shared.Caching.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Collections.Concurrent;

namespace Convex.Shared.Caching.Services;

/// <summary>
/// High-performance cache service for billion-record scenarios
/// </summary>
public class ConvexCache : IConvexCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentDictionary<string, Task> _pendingOperations;

    public ConvexCache(IDistributedCache distributedCache, int maxConcurrency = 1000)
    {
        _distributedCache = distributedCache;
        _semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        _pendingOperations = new ConcurrentDictionary<string, Task>();
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false, // Performance optimization
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        await _semaphore.WaitAsync();
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value, _jsonOptions);
        }
        catch
        {
            return default;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        await _semaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            await _distributedCache.SetStringAsync(key, json, options);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _distributedCache.RemoveAsync(key);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        await _semaphore.WaitAsync();
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }
        catch
        {
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
            return cachedValue;

        var newValue = await factory();
        await SetAsync(key, newValue, expiration);
        return newValue;
    }

    public async Task<int> RemoveManyAsync(params string[] keys)
    {
        // Process in parallel batches for better performance with billion records
        const int batchSize = 100;
        var batches = keys.Chunk(batchSize);
        var tasks = batches.Select(batch => ProcessBatchAsync(batch.ToArray()));
        
        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    private async Task<int> ProcessBatchAsync(string[] keys)
    {
        var tasks = keys.Select(async key =>
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
                return 1;
            }
            catch
            {
                return 0;
            }
        });

        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    public async Task<Dictionary<string, T?>> GetManyAsync<T>(params string[] keys)
    {
        // Process in parallel batches for better performance with billion records
        const int batchSize = 100;
        var batches = keys.Chunk(batchSize);
        var tasks = batches.Select(batch => GetBatchAsync<T>(batch.ToArray()));
        
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(dict => dict).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private async Task<Dictionary<string, T?>> GetBatchAsync<T>(string[] keys)
    {
        var tasks = keys.Select(async key =>
        {
            var value = await GetAsync<T>(key);
            return new KeyValuePair<string, T?>(key, value);
        });

        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public async Task<int> SetManyAsync<T>(Dictionary<string, T> items, TimeSpan? expiration = null)
    {
        if (items == null || items.Count == 0)
            return 0;

        // Process in parallel batches for better performance with billion records
        const int batchSize = 100;
        var batches = items.Chunk(batchSize);
        var tasks = batches.Select(batch => SetBatchAsync(batch.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), expiration));
        
        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    private async Task<int> SetBatchAsync<T>(Dictionary<string, T> items, TimeSpan? expiration)
    {
        var tasks = items.Select(async kvp =>
        {
            var success = await SetAsync(kvp.Key, kvp.Value, expiration);
            return success ? 1 : 0;
        });

        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }
}
