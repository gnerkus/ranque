using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Shared;

namespace Service
{
    internal sealed class OrganizationService : IOrganizationService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;

        public OrganizationService(IRepositoryManager repository, ILogger<IApiService> logger, IMapper
            mapper)
        {
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
            var org = await IsOrgExist(orgId, trackChanges);

            var orgDto = _mapper.Map<OrganizationDto>(org);
            return orgDto;
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrgForCreationDto orgDto)
        {
            var org = _mapper.Map<Organization>(orgDto);

            _repository.Organization.CreateOrganization(org);
            await _repository.SaveAsync();

            return _mapper.Map<OrganizationDto>(org);
        }

        public async Task UpdateOrganizationAsync(Guid orgId, OrgForUpdateDto orgForUpdateDto,
            bool trackChanges)
        {
            var org = await IsOrgExist(orgId, trackChanges);

            _mapper.Map(orgForUpdateDto, org);
            await _repository.SaveAsync();
        }

        public async Task DeleteOrganizationAsync(Guid orgId, bool trackChanges)
        {
            var org = await IsOrgExist(orgId, trackChanges);

            _repository.Organization.DeleteOrganization(org);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<OrganizationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool
            trackChanges)
        {
            if (ids is null) throw new IdParametersBadRequestException();

            var enumerable = ids.ToList();
            var dbOrgs = await _repository.Organization.GetByIdsAsync(enumerable, trackChanges);
            if (enumerable.Count != dbOrgs.Count()) throw new CollectionByIdsBadRequestException();

            return _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
        }

        public async Task<(IEnumerable<OrganizationDto> orgs, string ids)>
            CreateOrgCollectionAsync(
                IEnumerable<OrgForCreationDto> orgCollection)
        {
            if (orgCollection is null) throw new OrgCollectionBadRequestException();

            var dbOrgs = _mapper.Map<IEnumerable<Organization>>(orgCollection);
            foreach (var org in dbOrgs) _repository.Organization.CreateOrganization(org);

            await _repository.SaveAsync();
            var orgsDtos = _mapper.Map<IEnumerable<OrganizationDto>>(dbOrgs);
            var organizationDtos = orgsDtos.ToList();
            var ids = string.Join(",", organizationDtos.Select(c => c.Id));
            return (orgs: organizationDtos, ids);
        }

        private async Task<Organization> IsOrgExist(Guid orgId, bool trackChanges)
        {
            var org = await _repository.Organization.GetOrganizationAsync(orgId, trackChanges);
            if (org == null)
                throw new OrgNotFoundException(orgId);

            return org;
        }
    }
}