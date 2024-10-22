using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Entities.Test;

public class LeaderboardControllerTests
{
    [Fact]
    public async Task GET_retrieves_participants()
    {
        await using var app = new WebApplicationFactory<Program>();
        using var client = app.CreateClient();

        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        var response = await client.GetAsync($"api/leaderboards/{leaderboardId}/participants");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
