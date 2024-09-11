using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/organizations/{orgId}/participants")]
    [ApiController]
    public class ParticipantsController: ControllerBase
    {
        private readonly IServiceManager _service;

        public ParticipantsController(IServiceManager service)
        {
            _service = service;
        }
        
        [HttpGet]
        public IActionResult GetParticipantsForOrganization(Guid orgId)
        {
            var participants = _service.ParticipantService.GetParticipants(orgId, trackChanges:
                false);
            return Ok(participants);
        }

    }
}