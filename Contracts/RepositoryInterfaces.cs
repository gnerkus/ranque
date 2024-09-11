using Entities;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        IEnumerable<Organization> GetAllOrganizations(bool trackChanges);
        Organization? GetOrganization(Guid orgId, bool trackChanges);
    }

    public interface ILeaderboardRepository
    {
    }

    public interface IParticipantRepository
    {
        IEnumerable<Participant> GetParticipants(Guid orgId, bool trackChanges);
        Participant? GetParticipant(Guid orgId, Guid participantId, bool trackChanges);
    }
}