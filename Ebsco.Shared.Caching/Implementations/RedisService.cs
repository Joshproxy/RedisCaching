using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using Ebsco.Shared.Caching.Interfaces;

namespace Ebsco.Shared.Caching.Implementations
{
    public class RedisService<T2> : ICachingService<T2> where T2 : NamespacedKey<T2>, new()
    {
        private readonly IDatabase _redisDb;
        private readonly IErrorLogger _errorLogger;
        
        public RedisService(IDatabase redisDb, IErrorLogger errorLogger)
        {
            _redisDb = redisDb;
            _errorLogger = errorLogger;
        }

        public T Get<T>(string key, Object obj = null) where T : class
        {
            if (_redisDb == null) return null;
            return this.Get<T>(NamespacedKey<T2>.Create(key, obj));
        }

        public bool Set<T>(T value, string key, Object obj = null, TimeSpan? expiration = null) where T : class
        {
            if (_redisDb == null || value == null) return false;
            return this.Set<T>(NamespacedKey<T2>.Create(key, obj), value, expiration);
        }

        public T GetSet<T>(Func<T> getMethod, string key, Object obj = null, TimeSpan? expiration = null) where T : class
        {
            if (getMethod == null) return null;
            return this.GetSet(NamespacedKey<T2>.Create(key, obj), getMethod, expiration);
        }

        private void LogException(Exception ex)
        {
            if (_errorLogger != null)
            {
                try
                {
                    _errorLogger.LogError(ex);
                }
                catch
                {
                    // ignored
                }
            }
        }

        protected T Get<T>(T2 key) where T : class
        {
            if (_redisDb == null) return null;
            T t = null;
            try
            {
                string rValue = _redisDb.StringGet(key);
                if (rValue == null) return null;
                // Ensure the object hasn't grown stale in cache.
                // For example a deployment with property name changes.
                t = JsonConvert.DeserializeObject<T>(rValue);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return t;
        }

        protected bool Set<T>(T2 key, T value, TimeSpan? expiration = null) where T : class
        {
            if (_redisDb == null || value == null) return false;
            if(!expiration.HasValue) expiration = TimeSpan.FromDays(1);
            bool success = false;
            try
            {
                string sValue = JsonConvert.SerializeObject(value);
                success = _redisDb.StringSet(key, sValue, expiration);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return success;
        }

        protected T GetSet<T>(T2 key, Func<T> getMethod, TimeSpan? expiration = null) where T : class
        {
            var response = this.Get<T>(key);

            if (response == null)
            {
                response = getMethod();
                this.Set(key, response, expiration);
            }
            return response;
        }
    }

}
