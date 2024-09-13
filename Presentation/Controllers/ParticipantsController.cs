﻿using Contracts;
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

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteParticipantForOrg(Guid orgId, Guid id)
        {
            _service.ParticipantService.DeleteParticipantForOrg(orgId, id, false);
            return NoContent();
        }
    }
}