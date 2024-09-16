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

        public async Task<PagedList<Participant>> GetParticipantsAsync(Guid orgId, 
        ParticipantParameters parameters, bool trackChanges)
        {
            var items = await FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
            
            return PagedList<Participant>.ToPagedList(items, parameters.PageNumber, parameters.PageSize);
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