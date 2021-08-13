using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;

namespace Imitari.Services
{
    public interface ICacheService
    {
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, DateTimeOffset absoluteExpiration = default);
        T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTimeOffset absoluteExpiration);
        void Remove(string key);
    }
    public class CacheService : ICacheService
    {
        protected readonly IAppCache _memoryCache;
        public CacheService(IAppCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public virtual async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, DateTimeOffset absoluteExpiration = default)
        {
            if (absoluteExpiration == default)
            {
                absoluteExpiration = DateTimeOffset.Now.AddHours(1);
            }

            return await _memoryCache.GetOrAddAsync<T>(cacheKey, () => factory(), absoluteExpiration);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTimeOffset absoluteExpiration)
        {
            // locks get and set internally
            return _memoryCache.GetOrAdd<T>(cacheKey, () => factory(), absoluteExpiration);
        }

        private static class TypeLock<T>
        {
            public static object Lock { get; } = new object();
        }

    }
}
