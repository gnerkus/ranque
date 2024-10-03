using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared;

namespace Repository
{
    public class LeaderboardRepository : RepositoryBase<Leaderboard>, ILeaderboardRepository
    {
        public LeaderboardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Leaderboard>> GetAllLeaderboardsAsync
            (Guid orgId, LeaderboardParameters parameters, bool trackChanges)
        {
            var items = await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .Sort(parameters.OrderBy)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .CountAsync();

            return new PagedList<Leaderboard>(items, count, parameters.PageNumber,
                parameters.PageSize);
        }

        public async Task<Leaderboard?> GetLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId,
            bool
                trackChanges)
        {
            return await FindByCondition(c => c.OrganizationId.Equals(orgId) && c.Id.Equals
                        (leaderboardId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<Leaderboard?> GetLeaderboardAsync(Guid leaderboardId, bool
            trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals
                        (leaderboardId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Participant>> GetParticipantsAsync(Guid leaderboardId, bool trackChanges)
        {
            var leaderboard = await FindByCondition(c => c.Id.Equals(leaderboardId),
                    trackChanges)
                .Include(p => p.Participants)
                .SingleOrDefaultAsync();

            return leaderboard != null ? leaderboard.Participants : new List<Participant>();
        }

        public void CreateLeaderboard(Guid orgId, Leaderboard leaderboard)
        {
            leaderboard.OrganizationId = orgId;
            Create(leaderboard);
        }

        public void DeleteLeaderboard(Leaderboard leaderboard)
        {
            Delete(leaderboard);
        }
    }
}