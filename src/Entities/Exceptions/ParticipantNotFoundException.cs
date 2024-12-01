namespace Entities.Exceptions
{
    public sealed class ParticipantNotFoundException : NotFoundException
    {
        public ParticipantNotFoundException(Guid pcptId) : base(
            $"The participant with id: {pcptId} doesn't exist in the database.")
        {
        }
    }
}