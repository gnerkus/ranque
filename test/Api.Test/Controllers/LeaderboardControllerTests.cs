using Contracts;
using Moq;
using Presentation.Controllers;
using Shared;

namespace Api.Test.Controllers;
    
public class LeaderboardControllerTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly LeaderboardController _controller;

    public LeaderboardControllerTests()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _controller = new LeaderboardController(_mockServiceManager.Object);
    }

    [Fact]
    public async Task Get_GetParticipants_Success()
    {
        var leaderboardId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        _mockServiceManager.Setup(serviceManager =>
                serviceManager.LeaderboardService.GetParticipantsAsync(leaderboardId, false))
            .ReturnsAsync(new List<ParticipantDto> { new (participantId, "John Doe", 20, "Manager")});

        // _controller is the SUT (System Under Test)
        var result = await _controller.GetParticipants(leaderboardId);

        Assert.NotNull(result);
    }
}
