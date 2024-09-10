using Contracts;
using Entities;
using Shared;

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

        public IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges)
        {
            try
            {
                var orgs = _repository.Organization.GetAllOrganizations(trackChanges);
                var orgsDto = orgs.Select(c => new OrganizationDto(c.Id, c.Name ?? "", string
                    .Join(' ', c.Address, c.Country))).ToList();
                return orgsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllOrganizations)} service method {ex}");
                throw;
            }
        }
    }
}