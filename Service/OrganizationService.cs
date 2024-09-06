using Contracts;

namespace Service
{
    internal sealed class OrganizationService : IOrganizationService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public OrganizationService(IRepositoryManager repository, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
        }
    }
}