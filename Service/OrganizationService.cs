using AutoMapper;
using Contracts;
using Shared;

namespace Service
{
    internal sealed class OrganizationService : IOrganizationService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public OrganizationService(IRepositoryManager repository, ILoggerManager logger, IMapper
         mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges)
        {
            try
            {
                var orgs = _repository.Organization.GetAllOrganizations(trackChanges);
                var orgsDto = _mapper.Map<IEnumerable<OrganizationDto>>(orgs);
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