using Entities.Models;

namespace Repository.Extensions
{
    public static class RepositoryScoreExtensions
    {
        public static IQueryable<Score> FilterScores(this IQueryable<Score> scores, Guid?
            leaderboardId, Guid? participantId)
        {
            var result = scores;

            if (leaderboardId != null && leaderboardId != Guid.Empty)
                result = result.Where(e => e.LeaderboardId == leaderboardId);

            if (participantId != null && participantId != Guid.Empty)
                result = result.Where(e => e.ParticipantId == participantId);

            return result;
        }
    }
}