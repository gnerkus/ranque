using System.Text.Json;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/leaderboards/{leaderboardId:guid}")]
    [Authorize(Roles = "Manager")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public LeaderboardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("scores", Name = "GetLeaderboardScores")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardScores(Guid leaderboardId,
            [FromQuery] ScoreParameters parameters)
        {
            var linkParams = new ScoreLinkParams(parameters, HttpContext);
            var pagedResult = await _service.ScoreService.GetLeaderboardScoresAsync
                (leaderboardId, linkParams, false);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult
                .metaData));

            return pagedResult.linkResponse.HasLinks
                ? Ok(pagedResult.linkResponse
                    .LinkedEntities)
                : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("participants", Name = "GetParticipants")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetParticipants(Guid leaderboardId)
        {
            var participants = await _service.LeaderboardService.GetParticipantsAsync
                (leaderboardId, false);
            return Ok(participants);
        }
    }
}