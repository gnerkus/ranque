using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Api.Test.Entities;

public class LeaderboardTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Test Leaderboard", true)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
    public void Leaderboard_Name_Max50Chars(string leaderboardName, bool expectedResult)
    {
        var leaderboard = new Leaderboard
        {
            Name = leaderboardName
        };
        var context = new ValidationContext(leaderboard);
        var results = new List<ValidationResult>();

        var actual = Validator.TryValidateObject(leaderboard, context, results, true);
        Assert.Equal(actual, expectedResult);
    }
}
