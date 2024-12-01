namespace Entities.Exceptions
{
    public sealed class ScoreNotFoundException : NotFoundException
    {
        public ScoreNotFoundException(Guid scoreId) : base(
            $"The score with id: {scoreId} doesn't exist in the database.")
        {
        }
    }
}