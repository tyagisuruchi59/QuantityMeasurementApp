using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementAppRepositoryLayer.Utilities
{
    /// <summary>
    /// Redis caching service using IDistributedCache.
    /// Provides generic get/set/delete with TTL support.
    ///
    /// Use cases:
    /// - Cache quantity measurement history (reduce DB reads)
    /// - JWT blacklist for logout/revoke
    /// - Operation count caching
    /// </summary>
    public class RedisService
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly ILogger<RedisService> _logger;

        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan JwtBlacklistTtl = TimeSpan.FromHours(2);

        public RedisService(
            IDistributedCache cache,
            IConfiguration config,
            ILogger<RedisService> logger)
        {
            _cache = cache;
            _config = config;
            _logger = logger;
        }

        // ─── Generic Get/Set/Delete ────────────────────────────────────────────

        /// <summary>
        /// Get a value from cache and deserialize it.
        /// Returns default(T) if key not found.
        /// </summary>
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (json is null)
                {
                    _logger.LogDebug("Cache MISS for key: {Key}", key);
                    return default;
                }
                _logger.LogDebug("Cache HIT for key: {Key}", key);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GET error for key: {Key}", key);
                return default;
            }
        }

        /// <summary>
        /// Set a value in cache with optional TTL.
        /// Serializes the value to JSON automatically.
        /// </summary>
        public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl ?? DefaultTtl
                };
                await _cache.SetStringAsync(key, json, options);
                _logger.LogDebug("Cache SET key: {Key}, TTL: {Ttl}", key, ttl ?? DefaultTtl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SET error for key: {Key}", key);
            }
        }

        /// <summary>
        /// Delete a key from cache.
        /// </summary>
        public async Task DeleteAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogDebug("Cache DELETE key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis DELETE error for key: {Key}", key);
            }
        }

        /// <summary>
        /// Check if a key exists in the cache.
        /// </summary>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                return value is not null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis EXISTS check error for key: {Key}", key);
                return false;
            }
        }

        // ─── JWT Blacklist ─────────────────────────────────────────────────────

        /// <summary>
        /// Add a JWT token ID (jti) to the blacklist.
        /// Used when a user logs out to invalidate their token.
        /// </summary>
        public async Task BlacklistTokenAsync(string jti, TimeSpan? ttl = null)
        {
            var key = $"blacklist:{jti}";
            await SetAsync(key, true, ttl ?? JwtBlacklistTtl);
            _logger.LogInformation("JWT blacklisted: jti={Jti}", jti);
        }

        /// <summary>
        /// Check if a JWT token ID is blacklisted.
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            var key = $"blacklist:{jti}";
            return await ExistsAsync(key);
        }

        // ─── Cache Key Helpers ─────────────────────────────────────────────────

        public static string HistoryKey(string operation) => $"history:op:{operation.ToLower()}";
        public static string TypeHistoryKey(string type) => $"history:type:{type.ToLower()}";
        public static string CountKey(string operation) => $"count:op:{operation.ToLower()}";
        public static string UserKey(string email) => $"user:{email.ToLower()}";
    }
}