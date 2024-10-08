namespace Entities.Exceptions
{
    public sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException() : base("Parameter ids is null")
        {
        }
    }

    public sealed class CollectionByIdsBadRequestException : BadRequestException
    {
        public CollectionByIdsBadRequestException()
            : base("Collection count mismatch comparing to ids.")
        {
        }
    }

    public sealed class OrgCollectionBadRequestException : BadRequestException
    {
        public OrgCollectionBadRequestException() : base("Company collection sent from a client is null.")
        {
        }
    }
}