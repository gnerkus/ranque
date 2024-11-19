using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(RepositoryContext repositoryContext) : base(
            repositoryContext)
        {
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync(string ownerId, bool
            trackChanges)
        {
            return await FindByCondition(o => o.OwnerId.Equals(ownerId), trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Organization?> GetOrganizationAsync(string ownerId, Guid orgId,
            bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(orgId) && c.OwnerId.Equals(ownerId),
                    trackChanges)
                .SingleOrDefaultAsync();
        }

        public void CreateOrganization(string ownerId, Organization org)
        {
            org.OwnerId = ownerId;
            Create(org);
        }

        public void DeleteOrganization(Organization org)
        {
            Delete(org);
        }

        public async Task<IEnumerable<Organization>> GetByIdsAsync(string ownerId,
            IEnumerable<Guid> ids,
            bool
                trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id) && x.OwnerId.Equals(ownerId),
                    trackChanges)
                .ToListAsync();
        }
    }
}