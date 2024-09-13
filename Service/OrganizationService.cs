using AutoMapper;
using Contracts;
using Entities;
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

        public OrganizationDto CreateOrganization(OrgForCreationDto orgDto)
        {
            var org = _mapper.Map<Organization>(orgDto);
            
            _repository.Organization.CreateOrganization(org);
            _repository.Save();

            return _mapper.Map<OrganizationDto>(org);
        }

        public IEnumerable<OrganizationDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }

            var dbOrgs = _repository.Organization.GetByIds(ids, trackChanges);
            if (ids.Count() != dbOrgs.Count())
            {
                throw new CollectionByIdsBadRequestException();
            }

            return _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
        }

        public (IEnumerable<OrganizationDto> orgs, string ids) CreateOrgCollection(IEnumerable<OrgForCreationDto> orgCollection)
        {
            if (orgCollection is null)
            {
                throw new OrgCollectionBadRequest();
            }

            var dbOrgs = _mapper.Map<IEnumerable<Organization>>(orgCollection);
            foreach (var org in dbOrgs)
            {
                _repository.Organization.CreateOrganization(org);
            }
            
            _repository.Save();
            var orgsDtos = _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
            var organizationDtos = orgsDtos.ToList();
            var ids = string.Join(",", organizationDtos.Select(c => c.Id));
            return (orgs: organizationDtos, ids: ids);
        }
    }
}