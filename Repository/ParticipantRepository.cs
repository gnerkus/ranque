using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Repository
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Participant>> GetParticipantsAsync(Guid orgId, 
        ParticipantParameters parameters, bool trackChanges)
        {
            return await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .OrderBy(c => c.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<Participant?> GetParticipantAsync(Guid orgId, Guid participantId, bool 
        trackChanges)
        {
            return await FindByCondition(
                    c => c.OrganizationId.Equals(orgId) && c.Id.Equals(participantId),
                    trackChanges)
                .SingleOrDefaultAsync();
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