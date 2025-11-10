using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {

        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync<T>(string cacheKey, T response, TimeSpan timeToLive)
        {
            if (response is null)
                return;

            // to convert data with property Keys to Camel case as it can be read from Javascript
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            string serializedResponse = JsonSerializer.Serialize(response, options); // to make response to JSON

            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            RedisValue cachedResponse = await _database.StringGetAsync(cacheKey);// RedisValue is struct

            if (cachedResponse.IsNullOrEmpty) // struct cannot be null
                return null;
            else
                return cachedResponse;
        }
    }
}
