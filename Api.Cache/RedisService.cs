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

    public void UpdateScore(Guid leaderboardId, Guid participantId, int increment)
    {
        _db.SortedSetIncrement(leaderboardId.ToString(), participantId.ToString(), increment);
    }

    public IEnumerable<SortedSetEntry> GetLeaderboard(Guid leaderboardId)
    {
        return _db.SortedSetScan(leaderboardId.ToString());
    }
}