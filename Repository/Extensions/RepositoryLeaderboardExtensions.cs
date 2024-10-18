using System.Linq.Dynamic.Core;
using Entities;
using Entities.Models;

namespace Repository.Extensions
{
    public static class RepositoryLeaderboardExtensions
    {
        public static IQueryable<Leaderboard> Sort(this IQueryable<Leaderboard> leaderboards,
            string? orderByQueryString)
        {
            var orderedByName = leaderboards.OrderBy(e => e.Name);

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return orderedByName;

            var orderQuery = OrderQueryBuilder.CreateOrderByQuery<Leaderboard>(orderByQueryString);

            return string.IsNullOrWhiteSpace(orderQuery)
                ? orderedByName
                : leaderboards.OrderBy(orderQuery);
        }
    }
}