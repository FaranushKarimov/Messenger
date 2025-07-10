using Microsoft.Extensions.Caching.Distributed;

namespace Messenger.Infrastructure.Services;

public class RedisPresenceService
{
    private readonly IDistributedCache _cache;

    public RedisPresenceService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetUserOnlineAsync(string userId)
    {
        await _cache.SetStringAsync($"user:{userId}:status", "online");
    }

    public async Task SetUserOfflineAsync(string userId)
    {
        await _cache.SetStringAsync($"user:{userId}:status", "offline");
    }

    public async Task<string?> GetUserStatusAsync(string userId)
    {
        return await _cache.GetStringAsync($"user:{userId}:status");
    }
}