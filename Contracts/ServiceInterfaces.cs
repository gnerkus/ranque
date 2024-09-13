using Entities;
using Shared;

namespace Contracts
{
    public interface IOrganizationService
    {
        IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges);
        OrganizationDto GetOrganization(Guid orgId, bool trackChanges);
        OrganizationDto CreateOrganization(OrgForCreationDto orgDto);

        void UpdateOrganization(Guid orgId, OrgForUpdateDto orgForUpdateDto, bool trackChanges);

        void DeleteOrganization(Guid orgId, bool trackChanges);

        IEnumerable<OrganizationDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

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