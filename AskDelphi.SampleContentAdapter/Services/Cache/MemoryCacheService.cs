using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(CacheKey key)
        {
            string memoryCacheKey = key.AsString();
            if (!memoryCache.TryGetValue<T>(memoryCacheKey, out T result))
            {
                return await Task.FromResult(default(T));
            }
            return await Task.FromResult<T>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public Task<T> SetWithAbsoluteExpirationAsync<T>(CacheKey key, T value, DateTimeOffset expiresAt)
        {
            string memoryCacheKey = key.AsString();
            T result = memoryCache.Set<T>(memoryCacheKey, value, expiresAt);
            return Task.FromResult<T>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireAfter"></param>
        /// <returns></returns>
        public Task<T> SetWithSlidingExpiration<T>(CacheKey key, T value, TimeSpan expireAfter)
        {
            string memoryCacheKey = key.AsString();
            T result = memoryCache.Set<T>(memoryCacheKey, value, new MemoryCacheEntryOptions { SlidingExpiration = expireAfter });
            return Task.FromResult<T>(result);
        }
    }
}
