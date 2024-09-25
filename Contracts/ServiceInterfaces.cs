using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Contracts
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync(bool trackChanges);
        Task<OrganizationDto> GetOrganizationAsync(Guid orgId, bool trackChanges);
        Task<OrganizationDto> CreateOrganizationAsync(OrgForCreationDto orgDto);

        Task UpdateOrganizationAsync(Guid orgId, OrgForUpdateDto orgForUpdateDto, bool
            trackChanges);

        Task DeleteOrganizationAsync(Guid orgId, bool trackChanges);

        Task<IEnumerable<OrganizationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<OrganizationDto> orgs, string ids)> CreateOrgCollectionAsync
            (IEnumerable<OrgForCreationDto> orgCollection);
    }

    public interface ILeaderboardService
    {
    }

    public interface IScoreService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllScoresAsync
        (Guid leaderboardId, Guid participantId, ScoreLinkParams
                parameters, bool
                trackChanges);

        Task<ScoreDto> GetScoreAsync(Guid scoreId, bool trackChanges);

        Task<ScoreDto> CreateScoreAsync(Guid leaderboardId, Guid participantId,
            ScoreForManipulationDto scoreForCreationDto, bool trackChanges);

        Task DeleteScoreAsync(Guid scoreId, bool trackChanges);
        
        Task UpdateScoreAsync(Guid scoreId, ScoreForManipulationDto
            scoreForUpdateDto, bool trackChanges);
    }

    public interface IParticipantService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetParticipantsAsync
        (Guid orgId,
            LinkParameters
                parameters, bool
                trackChanges);

        Task<ParticipantDto> GetParticipantAsync(Guid orgId, Guid pcptId, bool trackChanges);

        Task<ParticipantDto> CreateParticipantForOrgAsync(Guid orgId, ParticipantForCreationDto
            participantForCreationDto, bool trackChanges);

        Task DeleteParticipantForOrgAsync(Guid orgId, Guid participantId, bool trackChanges);

        Task UpdateParticipantForOrgAsync(Guid orgId, Guid participantId, ParticipantForUpdateDto
            participantForUpdateDto, bool orgTrackChanges, bool pcptTrackChanges);

        Task<(ParticipantForUpdateDto participantToPatch, Participant participant)>
            GetParticipantForPatchAsync(
                Guid orgId, Guid participantId, bool orgTrackChanges, bool participantTrackChanges
            );

        Task SaveChangesForPatchAsync(ParticipantForUpdateDto participantToPatch,
            Participant participant);
    }

    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);

    }
}