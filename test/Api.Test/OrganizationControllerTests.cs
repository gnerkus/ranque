using System.Net.Http.Json;
using FluentAssertions;
using Shared;

namespace Entities.Test;

public class OrganizationControllerTests: IClassFixture<ApiTestWebApplicationFactory>
{
    private readonly ApiTestWebApplicationFactory _factory;

    public OrganizationControllerTests(ApiTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GET_retrieve_leaderboard()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        const string orgId = "7edac2a8-a73f-4926-8da3-fea7dbaf2ebd";
        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        
        // Act
        var leaderboardDto = await client.GetFromJsonAsync<LeaderboardDto>
            ($"api/orgs/{orgId}/leaderboards/{leaderboardId}");
        
        // Assert
        var leaderboard = new LeaderboardDto(new Guid(leaderboardId), "Product Sales");
        leaderboardDto.Should().NotBeNull()
            .And.BeEquivalentTo(leaderboard, options => options.Excluding(o => o.Id));
    }
}
