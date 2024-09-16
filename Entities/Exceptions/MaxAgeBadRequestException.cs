namespace Entities.Exceptions
{
    public sealed class MaxAgeBadRequestException : BadRequestException
    {
        public MaxAgeBadRequestException() : base("Max age can't be less than min age")
        {
        }
    }
}