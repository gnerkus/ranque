namespace Contracts
{
    public interface IServiceManager
    {
        IOrganizationService OrganizationService { get; }
        IParticipantService ParticipantService { get; }
        ILeaderboardService LeaderboardService { get; }
        
        IScoreService ScoreService { get; }
        
        IAuthenticationService AuthenticationService { get; }
    }
}