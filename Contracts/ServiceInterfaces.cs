using Entities;
using Shared;

namespace Contracts
{
    public interface IOrganizationService
    {
        IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges);
    }

    public interface ILeaderboardService
    {
    }

    public interface IParticipantService
    {
    }
}