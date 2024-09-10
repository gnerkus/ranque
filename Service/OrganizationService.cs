using Contracts;
using Entities;

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

        public IEnumerable<Organization> GetAllOrganizations(bool trackChanges)
        {
            try
            {
                var orgs = _repository.Organization.GetAllOrganizations(trackChanges);
                return orgs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllOrganizations)} service method {ex}");
                throw;
            }
        }
    }
}