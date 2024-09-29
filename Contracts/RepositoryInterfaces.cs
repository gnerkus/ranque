using Entities;
using Shared;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync(bool trackChanges);
        Task<Organization?> GetOrganizationAsync(Guid orgId, bool trackChanges);
        void CreateOrganization(Organization org);

        void DeleteOrganization(Organization org);

        Task<IEnumerable<Organization>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }

    public interface ILeaderboardRepository
    {
        Task<PagedList<Leaderboard>> GetAllLeaderboardsAsync(Guid orgId, LeaderboardParameters parameters,
            bool trackChanges);
        Task<Leaderboard?> GetLeaderboardAsync(Guid orgId, Guid leaderboardId, bool trackChanges);
        void CreateLeaderboard(Guid orgId, Leaderboard leaderboard);

        void DeleteLeaderboard(Leaderboard leaderboard);
    }

    public interface IScoreRepository
    {
        Task<PagedList<Score>> GetAllScoresAsync(ScoreParameters parameters,
            bool trackChanges);
        Task<Score?> GetScoreAsync(Guid scoreId, bool trackChanges);
        void CreateScore(Guid leaderboardId, Guid participantId, Score score);

        void DeleteScore(Score score);
    }

    public interface IParticipantRepository
    {
        Task<PagedList<Participant>> GetParticipantsAsync(Guid orgId,
            ParticipantParameters parameters,
            bool trackChanges);

        Task<Participant?> GetParticipantAsync(Guid orgId, Guid participantId, bool trackChanges);
        void CreateParticipant(Guid orgId, Participant participant);

        void DeleteParticipant(Participant participant);
    }
}