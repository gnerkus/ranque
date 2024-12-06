using System.Text.Json;
using Entities.Models;
using Shared;
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

    public void UpdateScore(Guid leaderboardId, Participant participant, double increment)
    {
        var participantKey = JsonSerializer.Serialize(participant);
        _db.SortedSetIncrement(leaderboardId.ToString(), participantKey, increment);
    }

    public IEnumerable<RankedParticipantDto> GetLeaderboard(Guid leaderboardId)
    {
        var leaderboard = _db.SortedSetScan(leaderboardId.ToString());

        return leaderboard.Select(sortedSetEntry =>
        {
            var participant = JsonSerializer.Deserialize<Participant>(sortedSetEntry.Element!);
            return new RankedParticipantDto
            {
                Id = participant.Id,
                Name = participant.Name,
                Score = sortedSetEntry.Score
            };
        });
    }
}