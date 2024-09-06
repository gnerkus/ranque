using Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ILeaderboardService> _leaderboardService;
        private readonly Lazy<IOrganizationService> _orgService;
        private readonly Lazy<IParticipantService> _participantService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager)
        {
            _orgService = new Lazy<IOrganizationService>(() => new OrganizationService
                (repositoryManager, loggerManager));
            _participantService = new Lazy<IParticipantService>(() => new ParticipantService
                (repositoryManager, loggerManager));
            _leaderboardService = new Lazy<ILeaderboardService>(() => new LeaderboardService
                (repositoryManager, loggerManager));
        }

        public IOrganizationService OrganizationService => _orgService.Value;
        public IParticipantService ParticipantService => _participantService.Value;
        public ILeaderboardService LeaderboardService => _leaderboardService.Value;
    }
}