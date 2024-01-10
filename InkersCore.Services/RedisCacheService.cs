using InkersCore.Domain.IServices;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace InkersCore.Services
{
    public class RedisCacheService : ITokenCacheService
    {
        private readonly ConnectionMultiplexer _redisServer;
        private readonly ILoggerService<RedisCacheService> _logger;

        public RedisCacheService(ILoggerService<RedisCacheService> logger, IConfiguration configuration)
        {
            _redisServer = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { configuration.GetValue<string>("RedisCache:ConnectionString") }
            });
            _logger = logger;
        }

        /// <summary>
        /// Function to insert key/value into cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Status</returns>
        public bool Insert(string key, string value)
        {
            try
            {
                var redis = _redisServer.GetDatabase();
                return redis.StringSet(key, value);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }

        }

        /// <summary>
        /// Function to remove key/value from cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Status</returns>
        public bool Delete(string key)
        {
            try
            {
                var redis = _redisServer.GetDatabase();
                return redis.KeyDelete(key);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to fetch value from cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public string? FetchValue(string key)
        {
            try
            {
                var redis = _redisServer.GetDatabase();
                return redis.StringGet(key);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }
    }
}
