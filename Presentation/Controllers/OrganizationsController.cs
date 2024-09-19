﻿using Contracts;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/orgs")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public OrganizationsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
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

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOrganization([FromBody] OrgForCreationDto orgDto)
        {
            var org = await _service.OrganizationService.CreateOrganizationAsync(orgDto);
            return CreatedAtRoute("OrgById", new { id = org.Id }, org);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody]
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

        [HttpOptions]
        public IActionResult GetOrgOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

            return Ok();
        }
    }
}