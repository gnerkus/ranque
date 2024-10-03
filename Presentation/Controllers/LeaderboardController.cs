using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/leaderboards")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public LeaderboardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("{leaderboardId:guid}/scores", Name = "GetLeaderboardScores")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardScores(Guid leaderboardId,
            [FromQuery] ScoreParameters parameters)
        {
            var linkParams = new ScoreLinkParams(parameters, HttpContext);
            var pagedResult = await _service.ScoreService.GetLeaderboardScoresAsync
                (leaderboardId, linkParams, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return pagedResult.linkResponse.HasLinks
                ? Ok(pagedResult.linkResponse
                    .LinkedEntities)
                : Ok(pagedResult.linkResponse.ShapedEntities);
        }
    }
}