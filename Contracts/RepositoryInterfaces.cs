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