using Entities.Models;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Contracts
{
    public interface IParticipantLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<ParticipantDto> participantsDto,
            string fields, Guid orgId, HttpContext httpContext);
    }

    public interface IScoreLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<ScoreDto> scoresDto,
            string fields, HttpContext httpContext);
    }

    public interface ILeaderboardLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<LeaderboardDto> leaderboardsDto,
            string fields, Guid orgId, HttpContext httpContext);
    }
}