using Entities;

namespace Contracts
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetAllOrganizations(bool trackChanges);
    }

    public interface ILeaderboardService
    {
    }

    public interface IParticipantService
    {
    }
}