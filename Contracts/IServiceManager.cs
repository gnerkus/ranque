namespace Contracts
{
    public interface IServiceManager
    {
        IOrganizationService OrganizationService { get; }
        IParticipantService ParticipantService { get; }
        ILeaderboardService LeaderboardService { get; }
        
        IAuthenticationService AuthenticationService { get; }
    }
}