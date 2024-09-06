using Contracts;

namespace Service
{
    internal sealed class LeaderboardService : ILeaderboardService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public LeaderboardService(IRepositoryManager repository, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
        }
    }
}