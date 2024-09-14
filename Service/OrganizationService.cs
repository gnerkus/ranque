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

        public async Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync(bool trackChanges)
        {
            var orgs = await _repository.Organization.GetAllOrganizationsAsync(trackChanges);
            var orgsDto = _mapper.Map<IEnumerable<OrganizationDto>>(orgs);
            return orgsDto;
        }

        public async Task<OrganizationDto> GetOrganizationAsync(Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
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

        public async void UpdateOrganizationAsync(Guid orgId, OrgForUpdateDto orgForUpdateDto,
            bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
            if (org == null)
                throw new OrgNotFoundException(orgId);

            _mapper.Map(orgForUpdateDto, org);
            _repository.Save();
        }

        public async void DeleteOrganizationAsync(Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
            if (org == null)
                throw new OrgNotFoundException(orgId);

            _repository.Organization.DeleteOrganization(org);
            _repository.Save();
        }

        public async Task<IEnumerable<OrganizationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool 
        trackChanges)
        {
            if (ids is null) throw new IdParametersBadRequestException();

            var dbOrgs = await _repository.Organization.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != dbOrgs.Count()) throw new CollectionByIdsBadRequestException();

            return _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
        }

        public (IEnumerable<OrganizationDto> orgs, string ids) CreateOrgCollection(
            IEnumerable<OrgForCreationDto> orgCollection)
        {
            if (orgCollection is null) throw new OrgCollectionBadRequest();

            var dbOrgs = _mapper.Map<IEnumerable<Organization>>(orgCollection);
            foreach (var org in dbOrgs) _repository.Organization.CreateOrganization(org);

            _repository.Save();
            var orgsDtos = _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
            var organizationDtos = orgsDtos.ToList();
            var ids = string.Join(",", organizationDtos.Select(c => c.Id));
            return (orgs: organizationDtos, ids);
        }
    }
}