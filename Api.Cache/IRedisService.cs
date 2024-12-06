using StackExchange.Redis;

namespace Api.Cache
{
    public interface IRedisService
    {
        public void UpdateScore(Guid leaderboardId, Guid participantId, int increment);
        public IEnumerable<SortedSetEntry> GetLeaderboard(Guid leaderboardId);
    }
}