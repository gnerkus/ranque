using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(RepositoryContext repositoryContext) : base(
            repositoryContext)
        {
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Organization?> GetOrganizationAsync(Guid orgId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(orgId), trackChanges)
                .SingleOrDefaultAsync();
        }

        public void CreateOrganization(Organization org)
        {
            Create(org);
        }

        public void DeleteOrganization(Organization org)
        {
            Delete(org);
        }

        public async Task<IEnumerable<Organization>> GetByIdsAsync(IEnumerable<Guid> ids, bool
            trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToListAsync();
        }
    }
}