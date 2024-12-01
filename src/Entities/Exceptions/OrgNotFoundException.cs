namespace Entities.Exceptions
{
    public sealed class OrgNotFoundException : NotFoundException
    {
        public OrgNotFoundException(Guid orgId) : base(
            $"The company with id: {orgId} doesn't exist in the database.")
        {
        }
    }
}