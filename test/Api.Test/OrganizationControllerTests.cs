using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Shared;
using Xunit.Abstractions;

namespace Entities.Test;

[Collection("IntegrationTests")]
public class OrganizationControllerTests(
    ApiTestWebApplicationFactory factory,
    ITestOutputHelper
        output)
    : IClassFixture<ApiTestWebApplicationFactory>
{
    [Fact]
    public async Task GET_retrieve_leaderboard()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        const string orgId = "7edac2a8-a73f-4926-8da3-fea7dbaf2ebd";
        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        
        // TODO: remove the next 5 lines after inspecting
        var getLeaderboardsMessage = new HttpRequestMessage(new HttpMethod("GET"),
            $"api/orgs/{orgId}/leaderboards/");
        var arrayResponse = await client.SendAsync(getLeaderboardsMessage);
        var arrayResult = await arrayResponse.Content.ReadAsStringAsync();
        output.WriteLine("WRITING LEADERBOARDS CONTENT");
        output.WriteLine(arrayResult);
        var getLeaderboardMessage = new HttpRequestMessage(new HttpMethod("GET"),
            $"api/orgs/{orgId}/leaderboards/{leaderboardId}");
        var response = await client.SendAsync(getLeaderboardMessage);
        output.WriteLine("WRITING RESPONSE CONTENT");
        output.WriteLine(response.ToString());
        var result = await response.Content.ReadAsStringAsync();
        output.WriteLine("WRITING RESULT CONTENT");
        output.WriteLine(result);
        JsonSerializerOptions seroptions = new()
        {
            IncludeFields = true,
        };
        var leaderboardDto = JsonSerializer.Deserialize<LeaderboardDto>(result, seroptions)!;
        output.WriteLine("WRITING DTO");
        output.WriteLine(leaderboardDto.ToString());
        // end TODO
        
        // Act
        // var leaderboardDto = await client.GetFromJsonAsync<LeaderboardDto>
        //     ($"api/orgs/{orgId}/leaderboards/{leaderboardId}");
        //
        // Assert
        var leaderboard = new LeaderboardDto(new Guid(leaderboardId), "Product Sales");
        leaderboardDto.Should().NotBeNull()
            .And.BeEquivalentTo(leaderboard, options => options.Excluding(o => o.Id));
    }
}
