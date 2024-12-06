using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Contracts
{
    public interface IApiService
    {
    }

    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync(string ownerId,
            bool trackChanges);

        Task<OrganizationDto> GetOrganizationAsync(string ownerId, Guid orgId, bool trackChanges);
        Task<OrganizationDto> CreateOrganizationAsync(string ownerId, OrgForCreationDto orgDto);

        Task UpdateOrganizationAsync(string ownerId, Guid orgId, OrgForUpdateDto orgForUpdateDto,
            bool
                trackChanges);

        Task DeleteOrganizationAsync(string ownerId, Guid orgId, bool trackChanges);

        Task<IEnumerable<OrganizationDto>> GetByIdsAsync(string ownerId, IEnumerable<Guid> ids,
            bool trackChanges);

        Task<(IEnumerable<OrganizationDto> orgs, string ids)> CreateOrgCollectionAsync
            (string ownerId, IEnumerable<OrgForCreationDto> orgCollection);
    }

    public interface ILeaderboardService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetLeaderboardsAsync
        (string userId, Guid orgId,
            LeaderboardLinkParams
                parameters, bool
                trackChanges);

        Task<RankedLeaderboardDto>
            GetLeaderboardAsync(string userId, Guid orgId, Guid leaderboardId, bool trackChanges);

        Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(Guid leaderboardId,
            bool trackChanges);

        Task<LeaderboardDto> CreateLeaderboardForOrgAsync(string userId, Guid orgId,
            LeaderboardForCreationDto
                leaderboardForCreationDto, bool trackChanges);

        Task DeleteLeaderboardForOrgAsync(string userId, Guid orgId, Guid leaderboardId,
            bool trackChanges);

        Task UpdateLeaderboardForOrgAsync(string userId, Guid orgId, Guid leaderboardId,
            LeaderboardForUpdateDto
                leaderboardForUpdateDto, bool orgTrackChanges, bool leaderboardTrackChanges);

        Task<(LeaderboardForUpdateDto leaderboardToPatch, Leaderboard leaderboard)>
            GetLeaderboardForPatchAsync(
                string userId, Guid orgId, Guid leaderboardId, bool orgTrackChanges,
                bool leaderboardTrackChanges
            );

        Task SaveChangesForPatchAsync(LeaderboardForUpdateDto leaderboardToPatch,
            Leaderboard leaderboard);
    }

    public interface IScoreService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllScoresAsync
        (ScoreLinkParams
            parameters, bool
            trackChanges);

        Task<ScoreDto> GetScoreAsync(Guid scoreId, bool trackChanges);

        Task<(LinkResponse linkResponse, MetaData metaData)> GetParticipantScoresAsync
        (Guid participantId, ScoreLinkParams
            parameters, bool
            trackChanges);

        Task<(LinkResponse linkResponse, MetaData metaData)> GetLeaderboardScoresAsync
        (Guid leaderboardId, ScoreLinkParams
            parameters, bool
            trackChanges);

        Task<bool> CheckScoreOrg(Guid leaderboardId, Guid participantId, bool trackChanges);

        Task<ScoreDto> CreateScoreAsync(Guid leaderboardId, Guid participantId,
            ScoreForCreationDto scoreForCreationDto, bool trackChanges);

        Task DeleteScoreAsync(Guid scoreId, bool trackChanges);

        Task UpdateScoreAsync(Guid scoreId, ScoreForManipulationDto
            scoreForUpdateDto, bool trackChanges);
    }

    public interface IParticipantService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetParticipantsAsync
        (string userId, Guid orgId,
            LinkParameters
                parameters, bool
                trackChanges);

        Task<ParticipantDto> GetParticipantAsync(string userId, Guid orgId, Guid pcptId, bool
            trackChanges);

        Task<IEnumerable<LeaderboardDto>> GetLeaderboardsAsync(Guid participantId,
            bool trackChanges);

        Task<ParticipantDto> CreateParticipantForOrgAsync(string userId, Guid orgId,
            ParticipantForCreationDto
                participantForCreationDto, bool trackChanges);

        Task DeleteParticipantForOrgAsync(string userId, Guid orgId, Guid participantId, bool
            trackChanges);

        Task UpdateParticipantForOrgAsync(string userId, Guid orgId, Guid participantId,
            ParticipantForUpdateDto
                participantForUpdateDto, bool orgTrackChanges, bool pcptTrackChanges);

        Task<(ParticipantForUpdateDto participantToPatch, Participant participant)>
            GetParticipantForPatchAsync(
                string userId, Guid orgId, Guid participantId, bool orgTrackChanges, bool
                    participantTrackChanges
            );

        Task SaveChangesForPatchAsync(ParticipantForUpdateDto participantToPatch,
            Participant participant);
    }

    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<User> GetAuthenticatedUser(string userId);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}