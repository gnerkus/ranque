using Contracts;

namespace Service
{
    internal sealed class ParticipantService : IParticipantService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ParticipantService(IRepositoryManager repository, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
        }
    }
}