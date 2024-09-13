using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared;

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
        public IActionResult GetParticipantsForOrganization(Guid orgId)
        {
            var participants = _service.ParticipantService.GetParticipants(orgId, false);
            return Ok(participants);
        }

        [HttpGet("{id:guid}", Name = "GetParticipantForOrg")]
        public IActionResult GetParticipantForOrganization(Guid orgId, Guid id)
        {
            var participant = _service.ParticipantService.GetParticipant(orgId, id,
                false);
            return Ok(participant);
        }

        [HttpPost]
        public IActionResult CreateParticipantForOrg(Guid orgId,
            [FromBody] ParticipantForCreationDto pcptDto)
        {
            if (pcptDto is null) return BadRequest("Participant creation request body is null");

            var participant = _service.ParticipantService.CreateParticipantForOrg(orgId,
                pcptDto, false);

            return CreatedAtRoute("GetParticipantForOrg", new { orgId, id = participant.Id },
                participant);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateParticipantForOrg(Guid orgId, Guid participantId,
            [FromBody] ParticipantForUpdateDto participantForUpdateDto)
        {
            if (participantForUpdateDto is null) return BadRequest("Participant body is null");

            _service.ParticipantService.UpdateParticipantForOrg(orgId, participantId,
                participantForUpdateDto, false, true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid orgId, Guid id,
            [FromBody] JsonPatchDocument<ParticipantForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");
            var result = _service.ParticipantService.GetParticipantForPatch(orgId, id,
                false,
                true);
            patchDoc.ApplyTo(result.participantToPatch);
            _service.ParticipantService.SaveChangesForPatch(result.participantToPatch,
                result.participant);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public IActionResult DeleteParticipantForOrg(Guid orgId, Guid id)
        {
            _service.ParticipantService.DeleteParticipantForOrg(orgId, id, false);
            return NoContent();
        }
    }
}