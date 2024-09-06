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
    }
}