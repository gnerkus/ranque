using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Presentation.Controllers
{
    [Route("api/orgs/{orgId}/participants")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ParticipantsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetParticipantsForOrganization(Guid orgId, [FromQuery] 
        ParticipantParameters parameters)
        {
            var pagedResult = await _service.ParticipantService.GetParticipantsAsync(orgId, parameters, 
            false);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            
            return Ok(pagedResult.participants);
        }

        [HttpGet("{id:guid}", Name = "GetParticipantForOrg")]
        public async Task<IActionResult> GetParticipantForOrganization(Guid orgId, Guid id)
        {
            var participant = await _service.ParticipantService.GetParticipantAsync(orgId, id,
                false);
            return Ok(participant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateParticipantForOrg(Guid orgId,
            [FromBody] ParticipantForCreationDto pcptDto)
        {
            if (pcptDto is null) return BadRequest("Participant creation request body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
                
            var participant = await _service.ParticipantService.CreateParticipantForOrgAsync(orgId,
                pcptDto, false);

            return CreatedAtRoute("GetParticipantForOrg", new { orgId, id = participant.Id },
                participant);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateParticipantForOrg(Guid orgId, Guid participantId,
            [FromBody] ParticipantForUpdateDto participantForUpdateDto)
        {
            if (participantForUpdateDto is null) return BadRequest("Participant body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            await _service.ParticipantService.UpdateParticipantForOrgAsync(orgId, participantId,
                participantForUpdateDto, false, true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateParticipantForCompany(Guid orgId, Guid id,
            [FromBody] JsonPatchDocument<ParticipantForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");
            var result = await _service.ParticipantService.GetParticipantForPatchAsync(orgId, id,
                false,
                true);
            patchDoc.ApplyTo(result.participantToPatch, ModelState);

            TryValidateModel(result.participantToPatch);
            
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            await _service.ParticipantService.SaveChangesForPatchAsync(result.participantToPatch,
                result.participant);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteParticipantForOrg(Guid orgId, Guid id)
        {
            await _service.ParticipantService.DeleteParticipantForOrgAsync(orgId, id, false);
            return NoContent();
        }
    }
}