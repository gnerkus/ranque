namespace Contracts
{
    public interface IRepositoryManager
    {
        IOrganizationRepository Organization { get; }
        ILeaderboardRepository Leaderboard { get; }
        IParticipantRepository Participant { get; }
        Task SaveAsync();
    }
}