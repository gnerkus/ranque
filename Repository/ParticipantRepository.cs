using Contracts;
using Entities;

namespace Repository
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}