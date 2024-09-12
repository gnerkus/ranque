using Contracts;
using Entities;

namespace Repository
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(RepositoryContext repositoryContext) : base(
            repositoryContext)
        {
        }

        public IEnumerable<Organization> GetAllOrganizations(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Organization? GetOrganization(Guid orgId, bool trackChanges)
        {
            return FindByCondition(c => c.Id.Equals(orgId), trackChanges)
                .SingleOrDefault();
        }

        public void CreateOrganization(Organization org)
        {
            Create(org);
        }

        public IEnumerable<Organization> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToList();
        }
    }
}