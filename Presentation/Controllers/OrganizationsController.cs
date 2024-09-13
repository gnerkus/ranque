using Contracts;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetOrganizations()
        {
            var orgs = _service.OrganizationService.GetAllOrganizations(false);
            return Ok(orgs);
        }

        [HttpGet("{id:guid}", Name = "OrgById")]
        public IActionResult GetOrganization(Guid id)
        {
            var org = _service.OrganizationService.GetOrganization(id, false);
            return Ok(org);
        }

        [HttpGet("collection/{{ids}}", Name = "OrgCollection")]
        public IActionResult GetOrgCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))
            ]
            IEnumerable<Guid> ids)
        {
            var orgs = _service.OrganizationService.GetByIds(ids, false);
            return Ok(orgs);
        }

        [HttpPost]
        public IActionResult CreateOrganization([FromBody] OrgForCreationDto orgDto)
        {
            if (orgDto is null) return BadRequest("Organization creation request body is null");

            var org = _service.OrganizationService.CreateOrganization(orgDto);
            return CreatedAtRoute("OrgById", new { id = org.Id }, org);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<OrgForCreationDto>
            orgCollection)
        {
            var result = _service.OrganizationService.CreateOrgCollection(orgCollection);

            return CreatedAtRoute("OrgCollection", new { result.ids }, result.orgs);
        }
    }
}