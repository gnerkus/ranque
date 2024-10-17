using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/participants/{participantId:guid}")]
    [Authorize(Roles = "Manager")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ParticipantsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("scores", Name = "GetParticipantScores")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetParticipantScores(Guid participantId,
            [FromQuery] ScoreParameters parameters)
        {
            var linkParams = new ScoreLinkParams(parameters, HttpContext);
            var pagedResult = await _service.ScoreService.GetParticipantScoresAsync
                (participantId, linkParams, false);

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metaData));

            return pagedResult.linkResponse.HasLinks
                ? Ok(pagedResult.linkResponse
                    .LinkedEntities)
                : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("leaderboards", Name = "GetLeaderboards")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboards(Guid participantId)
        {
            var leaderboards = await _service.ParticipantService.GetLeaderboardsAsync
                (participantId, false);
            return Ok(leaderboards);
        }
    }
}