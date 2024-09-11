namespace Entities.Exceptions
{
    public sealed class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid orgId) : base(
            $"The company with id: {orgId} doesn't exist in the database.")
        {
        }
    }
}