using Contracts;
using Entities;

namespace Repository
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Participant> GetParticipants(Guid orgId, bool trackChanges)
        {
            return FindByCondition(c => c.OrganizationId.Equals(orgId), trackChanges)
                .OrderBy(c => c.Name)
                .ToList();
        }
    }
}