using AutoMapper;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ILeaderboardService> _leaderboardService;
        private readonly Lazy<IOrganizationService> _orgService;
        private readonly Lazy<IParticipantService> _participantService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager
            loggerManager, IMapper mapper, IParticipantLinks participantLinks, UserManager<User>
             userManager, IConfiguration configuration)
        {
            _orgService = new Lazy<IOrganizationService>(() => new OrganizationService
                (repositoryManager, loggerManager, mapper));
            _participantService = new Lazy<IParticipantService>(() => new ParticipantService
                (repositoryManager, loggerManager, mapper, participantLinks));
            _leaderboardService = new Lazy<ILeaderboardService>(() => new LeaderboardService
                (repositoryManager, loggerManager, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new
                AuthenticationService(loggerManager, mapper, userManager, configuration));
        }

        public IOrganizationService OrganizationService => _orgService.Value;
        public IParticipantService ParticipantService => _participantService.Value;
        public ILeaderboardService LeaderboardService => _leaderboardService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}