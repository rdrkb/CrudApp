using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Database.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _redisDatabase;
        public RedisCache(IConfiguration configuration)
        {
            // Configure Redis
            var redis = ConnectionMultiplexer.Connect(configuration["RedisConfig:ConnectionString"]!);
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<T?> GetData<T>(string key)
        {
            var value = await _redisDatabase.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
                return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(value));
            return default;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _redisDatabase.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime, string paginationKey)
        {
            await _redisDatabase.ListRightPushAsync(paginationKey, key);
            long queuelength = _redisDatabase.ListLength(paginationKey);
            if (queuelength > 5)
            {
                await _redisDatabase.ListLeftPopAsync(paginationKey);
            }

            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _redisDatabase.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }

        public async Task<object> RemoveData(string key)
        {
            var _exist = await _redisDatabase.KeyExistsAsync(key);
            if (_exist)
                return await _redisDatabase.KeyDeleteAsync(key);
            return false;
        }

        public async Task RemoveAllData()
        {
            await _redisDatabase.ExecuteAsync("FLUSHDB");
        }
    }
}
