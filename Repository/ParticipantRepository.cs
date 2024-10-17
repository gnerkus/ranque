using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared;

namespace Repository
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Participant>> GetParticipantsAsync(Guid orgId,
            ParticipantParameters parameters, bool trackChanges)
        {
            var items = await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .FilterParticipants(parameters.MinAge, parameters.MaxAge)
                .Search(parameters.SearchTerm)
                .Sort(parameters.OrderBy)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .CountAsync();

            return new PagedList<Participant>(items, count, parameters.PageNumber,
                parameters.PageSize);
        }

        public async Task<Participant?> GetParticipantForOrgAsync(Guid orgId, Guid participantId,
            bool
                trackChanges)
        {
            return await FindByCondition(
                    c => c.OrganizationId.Equals(orgId) && c.Id.Equals(participantId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<Participant?> GetParticipantAsync(Guid participantId, bool
            trackChanges)
        {
            return await FindByCondition(
                    c => c.Id.Equals(participantId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Leaderboard>> GetLeaderboardsAsync(Guid participantId,
            bool trackChanges)
        {
            var participant = await FindByCondition(c => c.Id.Equals(participantId),
                    trackChanges)
                .Include(p => p.Leaderboards)
                .SingleOrDefaultAsync();

            return participant != null ? participant.Leaderboards : new List<Leaderboard>();
        }

        public void CreateParticipant(Guid orgId, Participant participant)
        {
            participant.OrganizationId = orgId;
            Create(participant);
        }

        public void DeleteParticipant(Participant participant)
        {
            Delete(participant);
        }
    }
}