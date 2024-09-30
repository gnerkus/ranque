using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/orgs/{orgId}/leaderboards")]
    [ApiController]
    public class LeaderboardController: ControllerBase
    {
        private readonly IServiceManager _service;

        public LeaderboardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardsForOrganization(Guid orgId, [FromBody] 
        LeaderboardParameters parameters)
        {
            var linkParams = new LeaderboardLinkParams(parameters, HttpContext);

            var pagedResult = await _service.LeaderboardService.GetLeaderboardsAsync(orgId,
                linkParams, false);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return pagedResult.linkResponse.HasLinks ? Ok(pagedResult.linkResponse
                .LinkedEntities) : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("{id:guid}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardForOrganization(Guid orgId, Guid id)
        {
            var leaderboard = await _service.LeaderboardService.GetLeaderboardAsync(orgId,
                id, false);

            return Ok(leaderboard);
        }
    }
}