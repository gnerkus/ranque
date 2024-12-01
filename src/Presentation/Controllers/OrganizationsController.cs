using System.Security.Claims;
using System.Text.Json;
using Asp.Versioning;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Shared;

namespace Presentation.Controllers
{
    [EnableCors("CorsPolicy")]
    [ApiVersion("1.0")]
    [Route("api/orgs")]
    [Authorize(Roles = "Manager")]
    [ApiController]
    [OutputCache(PolicyName = "120s")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public OrganizationsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("", Name = "GetOrganizations")]
        [EnableRateLimiting("SpecificPolicy")]
        public async Task<IActionResult> GetOrganizations()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            var orgs = await _service.OrganizationService.GetAllOrganizationsAsync(ownerId, false);
            return Ok(orgs);
        }

        [HttpGet("{id:guid}", Name = "OrgById")]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            var org = await _service.OrganizationService.GetOrganizationAsync(ownerId, id, false);
            return Ok(org);
        }

        [HttpGet("collection/{{ids}}", Name = "OrgCollection")]
        public async Task<IActionResult> GetOrgCollection([ModelBinder(BinderType = typeof
                (ArrayModelBinder))
            ]
            IEnumerable<Guid> ids)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            var orgs = await _service.OrganizationService.GetByIdsAsync(ownerId, ids, false);
            return Ok(orgs);
        }

        [HttpPost(Name = "CreateOrganization")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOrganization([FromBody] OrgForCreationDto orgDto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            var org = await _service.OrganizationService.CreateOrganizationAsync(ownerId, orgDto);
            return CreatedAtRoute("OrgById", new { id = org.Id }, org);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateOrganizationCollection([FromBody]
            IEnumerable<OrgForCreationDto>
                orgCollection)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            var result =
                await _service.OrganizationService.CreateOrgCollectionAsync(ownerId,
                    orgCollection);

            return CreatedAtRoute("OrgCollection", new { result.ids }, result.orgs);
        }

        [HttpPut("{orgId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateOrganization(Guid orgId, [FromBody] OrgForUpdateDto
            orgDto)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            await _service.OrganizationService.UpdateOrganizationAsync(ownerId, orgId, orgDto,
                true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrg(Guid id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId)) return Unauthorized("User is missing");

            await _service.OrganizationService.DeleteOrganizationAsync(ownerId, id, false);
            return NoContent();
        }

        //============== LEADERBOARDS =============================================

        [HttpGet("{organizationId:guid}/leaderboards", Name = "GetLeaderboardsForOrg")]
        [HttpHead("{organizationId:guid}/leaderboards")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardsForOrganization(Guid organizationId,
            [FromQuery] LeaderboardParameters parameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var linkParams = new LeaderboardLinkParams(parameters, HttpContext);

            var pagedResult = await _service.LeaderboardService.GetLeaderboardsAsync(userId,
                organizationId,
                linkParams, false);

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metaData));

            return pagedResult.linkResponse.HasLinks
                ? Ok(pagedResult.linkResponse
                    .LinkedEntities)
                : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("{orgId:guid}/leaderboards/{id:guid}", Name = "GetLeaderboardForOrg")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardForOrganization(Guid orgId, Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var leaderboard = await _service.LeaderboardService.GetLeaderboardAsync(userId, orgId,
                id, false);

            return Ok(leaderboard);
        }

        [HttpPost("{orgId:guid}/leaderboards")]
        public async Task<IActionResult> CreateLeaderboardForOrg(Guid orgId,
            [FromBody] LeaderboardForCreationDto leaderboardForCreationDto)
        {
            if (leaderboardForCreationDto is null)
                return BadRequest("Leaderboard creation request body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var leaderboard = await _service.LeaderboardService.CreateLeaderboardForOrgAsync(
                userId,
                orgId,
                leaderboardForCreationDto, false);

            return CreatedAtRoute("GetLeaderboardForOrg", new { orgId, id = leaderboard.Id },
                leaderboard);
        }

        [HttpPut("{orgId:guid}/leaderboards/{leaderboardId:guid}")]
        public async Task<IActionResult> UpdateLeaderboardForOrg(Guid orgId, Guid leaderboardId,
            [FromBody] LeaderboardForUpdateDto leaderboardForUpdateDto)
        {
            if (leaderboardForUpdateDto is null) return BadRequest("Leaderboard body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            await _service.LeaderboardService.UpdateLeaderboardForOrgAsync(userId, orgId,
                leaderboardId,
                leaderboardForUpdateDto, false, true);

            return NoContent();
        }

        [HttpPatch("{orgId:guid}/leaderboards/{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateLeaderboardForOrg(Guid orgId, Guid id,
            [FromBody] JsonPatchDocument<LeaderboardForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var result = await _service.LeaderboardService.GetLeaderboardForPatchAsync(userId,
                orgId, id,
                false,
                true);
            patchDoc.ApplyTo(result.leaderboardToPatch, ModelState);

            TryValidateModel(result.leaderboardToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.LeaderboardService.SaveChangesForPatchAsync(result.leaderboardToPatch,
                result.leaderboard);
            return NoContent();
        }

        [HttpDelete("{orgId:guid}/leaderboards/{id:guid}")]
        public async Task<IActionResult> DeleteLeaderboardForOrg(Guid orgId, Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            await _service.LeaderboardService.DeleteLeaderboardForOrgAsync(userId, orgId, id,
                false);
            return NoContent();
        }

        // ================ PARTICIPANTS =====================================

        [HttpGet("{orgId:guid}/participants", Name = "GetParticipantsForOrg")]
        [HttpHead("{orgId:guid}/participants")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetParticipantsForOrganization(Guid orgId,
            [FromQuery] ParticipantParameters parameters)
        {
            var linkParams = new LinkParameters(parameters, HttpContext);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var pagedResult = await _service.ParticipantService.GetParticipantsAsync(userId, orgId,
                linkParams,
                false);

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metaData));

            return pagedResult.linkResponse.HasLinks
                ? Ok(pagedResult.linkResponse
                    .LinkedEntities)
                : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("{orgId:guid}/participants/{id:guid}", Name = "GetParticipantForOrg")]
        public async Task<IActionResult> GetParticipantForOrganization(Guid orgId, Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var participant = await _service.ParticipantService.GetParticipantAsync(userId, orgId,
                id,
                false);
            return Ok(participant);
        }

        [HttpPost("{orgId:guid}/participants")]
        public async Task<IActionResult> CreateParticipantForOrg(Guid orgId,
            [FromBody] ParticipantForCreationDto pcptDto)
        {
            if (pcptDto is null) return BadRequest("Participant creation request body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var participant = await _service.ParticipantService.CreateParticipantForOrgAsync
            (userId,
                orgId,
                pcptDto, false);

            return CreatedAtRoute("GetParticipantForOrg", new { orgId, id = participant.Id },
                participant);
        }

        [HttpPut("{orgId:guid}/participants/{participantId:guid}")]
        public async Task<IActionResult> UpdateParticipantForOrg(Guid orgId, Guid participantId,
            [FromBody] ParticipantForUpdateDto participantForUpdateDto)
        {
            if (participantForUpdateDto is null) return BadRequest("Participant body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            await _service.ParticipantService.UpdateParticipantForOrgAsync(userId, orgId,
                participantId,
                participantForUpdateDto, false, true);

            return NoContent();
        }

        [HttpPatch("{orgId:guid}/participants/{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateParticipantForOrg(Guid orgId, Guid id,
            [FromBody] JsonPatchDocument<ParticipantForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var result = await _service.ParticipantService.GetParticipantForPatchAsync(userId,
                orgId,
                id,
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


        [HttpDelete("{orgId:guid}/participants/{id:guid}")]
        public async Task<IActionResult> DeleteParticipantForOrg(Guid orgId, Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            await _service.ParticipantService.DeleteParticipantForOrgAsync(userId, orgId, id,
                false);
            return NoContent();
        }

        //================== GENERAL ========================================

        [HttpOptions]
        public IActionResult GetOrgOptions()
        {
            Response.Headers.Append("Allow", "GET, OPTIONS, POST, PUT, DELETE");

            return Ok();
        }
    }
}