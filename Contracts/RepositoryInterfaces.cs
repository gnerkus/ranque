using Entities;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync(bool trackChanges);
        Task<Organization?> GetOrganizationAsync(Guid orgId, bool trackChanges);
        void CreateOrganization(Organization org);

        void DeleteOrganization(Organization org);

        Task<IEnumerable<Organization>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }

    public interface ILeaderboardRepository
    {
    }

    public interface IParticipantRepository
    {
        IEnumerable<Participant> GetParticipants(Guid orgId, bool trackChanges);
        Participant? GetParticipant(Guid orgId, Guid participantId, bool trackChanges);
        void CreateParticipant(Guid orgId, Participant participant);

        void DeleteParticipant(Participant participant);
    }
}