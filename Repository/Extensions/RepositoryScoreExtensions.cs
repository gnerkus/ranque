using System.Linq.Dynamic.Core;
using Entities;
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

        public static IQueryable<Score> Sort(this IQueryable<Score> scores,
            string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return scores.OrderBy(e => e.Value);

            var orderQuery = OrderQueryBuilder.CreateOrderByQuery<Score>(orderByQueryString);

            return string.IsNullOrWhiteSpace(orderQuery)
                ? scores.OrderBy(e => e.Value)
                : scores.OrderBy(orderQuery);
        }
    }
}