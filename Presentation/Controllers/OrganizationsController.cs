using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController: ControllerBase
    {
        private readonly IServiceManager _service;

        public OrganizationsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetOrganizations()
        {
            try
            {
                var orgs = _service.OrganizationService.GetAllOrganizations(trackChanges: false);
                return Ok(orgs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}