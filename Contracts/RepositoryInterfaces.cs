using Entities;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        IEnumerable<Organization> GetAllOrganizations(bool trackChanges);
    }

    public interface ILeaderboardRepository
    {
    }

    public interface IParticipantRepository
    {
    }
}