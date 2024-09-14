using Entities;
using Shared;

namespace Contracts
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync(bool trackChanges);
        Task<OrganizationDto> GetOrganizationAsync(Guid orgId, bool trackChanges);
        OrganizationDto CreateOrganization(OrgForCreationDto orgDto);

        void UpdateOrganizationAsync(Guid orgId, OrgForUpdateDto orgForUpdateDto, bool trackChanges);

        void DeleteOrganizationAsync(Guid orgId, bool trackChanges);

        Task<IEnumerable<OrganizationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        (IEnumerable<OrganizationDto> orgs, string ids) CreateOrgCollection
            (IEnumerable<OrgForCreationDto> orgCollection);
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

        void DeleteParticipantForOrg(Guid orgId, Guid participantId, bool trackChanges);

        void UpdateParticipantForOrg(Guid orgId, Guid participantId, ParticipantForUpdateDto
            participantForUpdateDto, bool orgTrackChanges, bool pcptTrackChanges);

        (ParticipantForUpdateDto participantToPatch, Participant participant)
            GetParticipantForPatch(
                Guid orgId, Guid participantId, bool orgTrackChanges, bool participantTrackChanges
            );

        void SaveChangesForPatch(ParticipantForUpdateDto participantToPatch,
            Participant participant);
    }
}