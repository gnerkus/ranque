using Contracts;
using Entities;

namespace Repository
{
    public class LeaderboardRepository: RepositoryBase<Leaderboard>, ILeaderboardRepository
    {
        public LeaderboardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}