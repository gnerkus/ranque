using Shared;

namespace Contracts
{
    public interface IOrganizationService
    {
        IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges);
        OrganizationDto GetOrganization(Guid orgId, bool trackChanges);
        OrganizationDto CreateOrganization(OrgForCreationDto orgDto);

        IEnumerable<OrganizationDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    }

    public interface ILeaderboardService
    {
    }

    public interface IParticipantService
    {
        IEnumerable<ParticipantDto> GetParticipants(Guid orgId, bool trackChanges);
        ParticipantDto GetParticipant(Guid orgId, Guid pcptId, bool trackChanges);

        ParticipantDto CreateParticipantForOrg(Guid orgId, ParticipantForCreationDto
            participantForCreationDto, bool trackChanges);
    }
}