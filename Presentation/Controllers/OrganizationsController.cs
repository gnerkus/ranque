using System.Text.Json;
using Asp.Versioning;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Shared;

namespace Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/orgs")]
    [ApiController]
    [OutputCache(PolicyName = "120s")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public OrganizationsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetOrganizations")]
        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("SpecificPolicy")]
        public async Task<IActionResult> GetOrganizations()
        {
            var orgs = await _service.OrganizationService.GetAllOrganizationsAsync(false);
            return Ok(orgs);
        }

        [HttpGet("{id:guid}", Name = "OrgById")]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            var org = await _service.OrganizationService.GetOrganizationAsync(id, false);

            return Ok(org);
        }

        [HttpGet("collection/{{ids}}", Name = "OrgCollection")]
        public async Task<IActionResult> GetOrgCollection([ModelBinder(BinderType = typeof
                (ArrayModelBinder))
            ]
            IEnumerable<Guid> ids)
        {
            var orgs = await _service.OrganizationService.GetByIdsAsync(ids, false);
            return Ok(orgs);
        }

        [HttpPost(Name = "CreateOrganization")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOrganization([FromBody] OrgForCreationDto orgDto)
        {
            var org = await _service.OrganizationService.CreateOrganizationAsync(orgDto);
            return CreatedAtRoute("OrgById", new { id = org.Id }, org);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateOrganizationCollection([FromBody]
            IEnumerable<OrgForCreationDto>
                orgCollection)
        {
            var result =
                await _service.OrganizationService.CreateOrgCollectionAsync(orgCollection);

            return CreatedAtRoute("OrgCollection", new { result.ids }, result.orgs);
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateOrganization(Guid orgId, [FromBody] OrgForUpdateDto
            orgDto)
        {
            await _service.OrganizationService.UpdateOrganizationAsync(orgId, orgDto, true);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrg(Guid id)
        {
            await _service.OrganizationService.DeleteOrganizationAsync(id, false);
            return NoContent();
        }
        
        //============== LEADERBOARDS =============================================

        [HttpGet("{orgId:guid}/leaderboards")]
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
        
        [HttpGet("{orgId:guid}/leaderboards/{id:guid}", Name = "GetLeaderboardForOrg")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetLeaderboardForOrganization(Guid orgId, Guid id)
        {
            var leaderboard = await _service.LeaderboardService.GetLeaderboardAsync(orgId,
                id, false);

            return Ok(leaderboard);
        }
        
        [HttpPost("{orgId:guid}/leaderboards")]
        public async Task<IActionResult> CreateLeaderboardForOrg(Guid orgId,
            [FromBody] LeaderboardForCreationDto leaderboardForCreationDto)
        {
            if (leaderboardForCreationDto is null) return BadRequest("Leaderboard creation request body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var leaderboard = await _service.LeaderboardService.CreateLeaderboardForOrgAsync(orgId,
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

            await _service.LeaderboardService.UpdateLeaderboardForOrgAsync(orgId, leaderboardId,
                leaderboardForUpdateDto, false, true);

            return NoContent();
        }
        
        [HttpPatch("{orgId:guid}/leaderboards/{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateLeaderboardForOrg(Guid orgId, Guid id,
            [FromBody] JsonPatchDocument<LeaderboardForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");
            var result = await _service.LeaderboardService.GetLeaderboardForPatchAsync(orgId, id,
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
            await _service.LeaderboardService.DeleteLeaderboardForOrgAsync(orgId, id, false);
            return NoContent();
        }
        
        //================== GENERAL ========================================

        [HttpOptions]
        public IActionResult GetOrgOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

            return Ok();
        }
    }
}