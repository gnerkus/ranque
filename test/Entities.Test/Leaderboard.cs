using Entities.Models;

namespace Entities.Test;

public class LeaderboardTests
{
    [Fact]
    public void Leaderboard_ShouldHave_Name()
    {
        var leaderboard = new Leaderboard();
        Assert.Null(leaderboard.Name);
    }
}
