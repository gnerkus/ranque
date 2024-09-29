namespace Entities.Exceptions
{
    public sealed class LeaderboardNotFoundException : NotFoundException
    {
        public LeaderboardNotFoundException(Guid pcptId) : base(
            $"The leaderboard with id: {pcptId} doesn't exist in the database.")
        {
        }
    }
}