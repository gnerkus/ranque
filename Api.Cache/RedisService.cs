using StackExchange.Redis;

namespace Api.Cache;

public class RedisService: IRedisService
{
    private readonly IDatabase _db;

    public RedisService()
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_REDIS_HOST");
        var redis = ConnectionMultiplexer.Connect(connectionString);
        _db = redis.GetDatabase();
    }
}