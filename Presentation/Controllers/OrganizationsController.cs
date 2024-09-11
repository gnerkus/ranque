using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id:guid}")]
        public IActionResult GetOrganization(Guid id)
        {
            var org = _service.OrganizationService.GetOrganization(id, false);
            return Ok(org);
        }
    }
}