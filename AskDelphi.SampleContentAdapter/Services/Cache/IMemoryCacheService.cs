using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemoryCacheService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(CacheKey key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        Task<T> SetWithAbsoluteExpirationAsync<T>(CacheKey key, T value, DateTimeOffset expiresAt);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireAfter"></param>
        /// <returns></returns>
        Task<T> SetWithSlidingExpiration<T>(CacheKey key, T value, TimeSpan expireAfter);
    }
}
