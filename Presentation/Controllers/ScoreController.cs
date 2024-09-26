using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/scores")]
    [ApiController]
    public class ScoreController: ControllerBase
    {
        private readonly IServiceManager _service;

        public ScoreController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetScoresAsync([FromQuery] ScoreParameters parameters)
        {
            var linkParams = new ScoreLinkParams(parameters, HttpContext);
            var pagedResult = await _service.ScoreService.GetAllScoresAsync(parameters
                .LeaderboardId, parameters.ParticipantId, linkParams, false);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            
            return pagedResult.linkResponse.HasLinks ? Ok(pagedResult.linkResponse
                .LinkedEntities) : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("{id:guid}", Name = "GetScore")]
        public async Task<IActionResult> GetScoreAsync(Guid id)
        {
            var score = await _service.ScoreService.GetScoreAsync(id, false);
            return Ok(score);
        }

        [HttpPut("{id:guid}", Name = "UpdateScore")]
        public async Task<IActionResult> UpdateScoreAsync(Guid id, [FromBody] ScoreForManipulationDto scoreForManipulationDto)
        {
            if (scoreForManipulationDto is null)
            {
                return BadRequest("Score body is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.ScoreService.UpdateScoreAsync(id, scoreForManipulationDto, true);

            return NoContent();
        }

        [HttpDelete("{id:guid}", Name = "DeleteScore")]
        public async Task<IActionResult> DeleteScoreAsync(Guid id)
        {
            await _service.ScoreService.DeleteScoreAsync(id, false);
            return NoContent();
        }
    }
}