using System.Security.Claims;
using Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

namespace Presentation.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IServiceManager _service;

        public UserController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is missing");

            var user = await _service.AuthenticationService.GetAuthenticatedUser(userId);
            return Ok(user);
        }
    }
}