using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Shared;

namespace Service
{
    internal sealed class OrganizationService : IOrganizationService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;

        public OrganizationService(IRepositoryManager repository, ILoggerManager logger, IMapper
            mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<OrganizationDto> GetAllOrganizations(bool trackChanges)
        {
            var orgs = _repository.Organization.GetAllOrganizations(trackChanges);
            var orgsDto = _mapper.Map<IEnumerable<OrganizationDto>>(orgs);
            return orgsDto;
        }

        public OrganizationDto GetOrganization(Guid orgId, bool trackChanges)
        {
            var org = _repository.Organization.GetOrganization(orgId, trackChanges);
            if (org == null)
                throw new OrgNotFoundException(orgId);
                
            var orgDto = _mapper.Map<OrganizationDto>(org);
            return orgDto;
        }
    }
}