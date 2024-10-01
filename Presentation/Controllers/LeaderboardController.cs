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
    public class LeaderboardController: ControllerBase
    {
        private readonly IServiceManager _service;

        public LeaderboardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost("{leaderboardId:guid}/participants/{participantId:guid}/scores")]
        public async Task<IActionResult> CreateScoreForParticipant(Guid leaderboardId, Guid 
        participantId, [FromBody] ScoreForManipulationDto scoreDto)
        {
            if (scoreDto is null) return BadRequest("Score creation request body is null");
            
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var score = await _service.ScoreService.CreateScoreAsync(leaderboardId,
                participantId, scoreDto, false);
            
            return CreatedAtRoute("GetScore", new { id = score.Id}, score);
        }
    }
}