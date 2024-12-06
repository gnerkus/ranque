using Entities.Models;
using Shared;

namespace Api.Cache
{
    public interface IRedisService
    {
        public void UpdateScore(Guid leaderboardId, Participant participant, double increment);
        public IEnumerable<RankedParticipantDto> GetLeaderboard(Guid leaderboardId);
    }
}