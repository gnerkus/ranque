using System.Net;
using FluentAssertions;

namespace Entities.Test;

public class LeaderboardControllerTests: IClassFixture<ApiTestWebApplicationFactory>
{
    private readonly ApiTestWebApplicationFactory _factory;

    public LeaderboardControllerTests(ApiTestWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GET_retrieves_participants()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        
        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        var request = new HttpRequestMessage(new HttpMethod("GET"),
            $"api/leaderboards/{leaderboardId}/participants");
        request.Headers.Add("Accept", "application/json");
        
        // Act
        var response = await client.SendAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
