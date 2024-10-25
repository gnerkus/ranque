using System.Net.Http.Json;
using FluentAssertions;
using Shared;

namespace Entities.Test;

[Collection("IntegrationTests")]
public class LeaderboardControllerTests(
    ApiTestWebApplicationFactory factory): IClassFixture<ApiTestWebApplicationFactory>
{
    [Fact]
    public async Task GET_retrieves_scores()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        const string participantId = "79e49410-c239-4443-bc96-30a515289c97";
        
        var createScoreRequest = new HttpRequestMessage(new HttpMethod("POST"),
            $"api/scores");
        createScoreRequest.Content = JsonContent.Create(new
        {
            value = 20,
            leaderboardId,
            participantId
        });
        
        // Act
        await client.SendAsync(createScoreRequest);
        var scoreDtos = await client.GetFromJsonAsync<IEnumerable<ScoreDto>>(
            $"api/leaderboards/{leaderboardId}/scores");
        
        // Assert
        var score = new ScoreDto(new Guid(participantId), 20);
        scoreDtos.Should().NotBeEmpty()
            .And.ContainSingle()
            .And.ContainEquivalentOf(score, options => options.Excluding(o => o.Id));
    }
    
    [Fact]
    public async Task GET_retrieves_participants()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-role", "Manager");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        const string leaderboardId = "a478da4c-a47b-4d95-896f-06368e844232";
        const string participantId = "79e49410-c239-4443-bc96-30a515289c97";
        
        var createScoreRequest = new HttpRequestMessage(new HttpMethod("POST"),
            $"api/scores");
        createScoreRequest.Content = JsonContent.Create(new
        {
            value = 20,
            leaderboardId,
            participantId
        });
        
        // Act
        await client.SendAsync(createScoreRequest);
        var participantDtos = await client.GetFromJsonAsync<IEnumerable<ParticipantDto>>(
            $"api/leaderboards/{leaderboardId}/participants");
        
        // Assert
        var participant = new ParticipantDto(new Guid(participantId), "Jane Smith", 28, "Software developer");
        participantDtos.Should().NotBeEmpty()
            .And.ContainEquivalentOf(participant);
    }
}
