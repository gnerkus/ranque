using System.Text.Json;
using Entities.Models;
using Shared;
using StackExchange.Redis;

namespace Api.Cache
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;

        public RedisService()
        {
            ConfigurationOptions ctx;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env == "Development")
                ctx = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { "localhost", 6379 }
                    }
                };
            else
                ctx = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        {
                            Environment.GetEnvironmentVariable("ASPNETCORE_REDIS_HOST")!, int
                                .Parse(Environment
                                    .GetEnvironmentVariable("ASPNETCORE_REDIS_PORT")!)
                        }
                    },
                    AbortOnConnectFail = false,
                    ConnectTimeout = 10000,
                    Ssl = true,
                    Password = Environment.GetEnvironmentVariable("ASPNETCORE_REDIS_PASSWORD")
                };

            var redis = ConnectionMultiplexer.Connect(ctx);
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
}