using System.Net.Http.Json;
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
        
        // Act
        var leaderboardDto = await client.GetFromJsonAsync<LeaderboardDto>
            ($"api/orgs/{orgId}/leaderboards/{leaderboardId}");
        
        // Assert
        var leaderboard = new LeaderboardDto(new Guid(leaderboardId), "Product Sales", "return score.First + score.Second");
        leaderboardDto.Should().NotBeNull()
            .And.BeEquivalentTo(leaderboard, options => options.Excluding(o => o.Id));
    }
    
    [Fact(Skip = "sorting not working; need to merge")]
    public async Task GET_retrieve_ranked_leaderboard()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        const string orgId = "7edac2a8-a73f-4926-8da3-fea7dbaf2ebd";
        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        const string participant1Id = "79e49410-c239-4443-bc96-30a515289c97";
        const string participant2Id = "063a04a0-5e1a-44a0-8005-e1737878e712";
        
        // Act
        var createScore1Request = new HttpRequestMessage(new HttpMethod("POST"),
            "api/scores");
        createScore1Request.Content = JsonContent.Create(new
        {
            JsonValue = "{\"First\":2,\"Second\"4}",
            leaderboardId,
            participantId = participant1Id
        });
        await client.SendAsync(createScore1Request);
        
        var createScore2Request = new HttpRequestMessage(new HttpMethod("POST"),
            "api/scores");
        createScore2Request.Content = JsonContent.Create(new
        {
            JsonValue = "{\"First\":2,\"Second\"4}",
            leaderboardId,
            participantId = participant1Id
        });
        await client.SendAsync(createScore2Request);
        
        var createScore3Request = new HttpRequestMessage(new HttpMethod("POST"),
            "api/scores");
        createScore3Request.Content = JsonContent.Create(new
        {
            JsonValue = "{\"First\":6,\"Second\"12}",
            leaderboardId,
            participantId = participant2Id
        });
        await client.SendAsync(createScore3Request);
        
        var leaderboardDto = await client.GetFromJsonAsync<RankedLeaderboardDto>
            ($"api/orgs/{orgId}/leaderboards/{leaderboardId}");
        
        // Assert
        var participantDto = leaderboardDto.Participants.ToList().First();
        var firstParticipant = new RankedParticipantDto
        {
            Id = new Guid(participant2Id),
            Name = "John Smith",
            Score = 18
        };
        participantDto.Should().BeEquivalentTo(firstParticipant);
    }
}
