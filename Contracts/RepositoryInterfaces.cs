using Entities.Models;
using Shared;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>>
            GetAllOrganizationsAsync(string ownerId, bool trackChanges);

        Task<Organization?> GetOrganizationAsync(string ownerId, Guid orgId, bool trackChanges);
        void CreateOrganization(string ownerId, Organization org);

        void DeleteOrganization(Organization org);

        Task<IEnumerable<Organization>> GetByIdsAsync(string ownerId, IEnumerable<Guid> ids, bool
            trackChanges);
    }

    public interface ILeaderboardRepository
    {
        Task<PagedList<Leaderboard>> GetAllLeaderboardsAsync(Guid orgId,
            LeaderboardParameters parameters,
            bool trackChanges);

        Task<Leaderboard?> GetLeaderboardForOrgAsync(Guid orgId, Guid leaderboardId,
            bool trackChanges);

        Task<Leaderboard?> GetLeaderboardAsync(Guid leaderboardId, bool trackChanges);
        Task<IEnumerable<Participant>> GetParticipantsAsync(Guid leaderboardId, bool trackChanges);

        void CreateLeaderboard(Guid orgId, Leaderboard leaderboard);

        void DeleteLeaderboard(Leaderboard leaderboard);
    }

    public interface IScoreRepository
    {
        Task<PagedList<Score>> GetAllScoresAsync(ScoreParameters parameters,
            bool trackChanges);

        Task<PagedList<Score>> GetParticipantScoresAsync(Guid participantId,
            ScoreParameters parameters, bool
                trackChanges);

        Task<PagedList<Score>> GetLeaderboardScoresAsync(Guid leaderboardId, ScoreParameters
            parameters, bool
            trackChanges);

        Task<Score?> GetScoreAsync(Guid scoreId, bool trackChanges);
        void CreateScore(Guid leaderboardId, Guid participantId, Score score);

        void DeleteScore(Score score);
    }

    public interface IParticipantRepository
    {
        Task<PagedList<Participant>> GetParticipantsAsync(Guid orgId,
            ParticipantParameters parameters,
            bool trackChanges);

        Task<Participant?> GetParticipantForOrgAsync(Guid orgId, Guid participantId,
            bool trackChanges);

        Task<Participant?> GetParticipantAsync(Guid participantId, bool trackChanges);

        Task<IEnumerable<Leaderboard>> GetLeaderboardsAsync(Guid participantId, bool trackChanges);

        void CreateParticipant(Guid orgId, Participant participant);

        void DeleteParticipant(Participant participant);
    }
}