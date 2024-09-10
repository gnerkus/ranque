using AutoMapper;
using Contracts;

namespace Service
{
    internal sealed class LeaderboardService : ILeaderboardService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public LeaderboardService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
    }
}