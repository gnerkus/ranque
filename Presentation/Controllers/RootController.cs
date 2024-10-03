using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("application/vnd.nanotome.apiroot"))
            {
                var list = new List<Link>
                {
                    new()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new { }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "GetOrganizations",
                            new { }),
                        Rel = "orgs",
                        Method = "GET"
                    },
                    new()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "CreateOrganization",
                            new { }),
                        Rel = "create_org",
                        Method = "POST"
                    }
                };

                return Ok(list);
            }

            return NoContent();
        }
    }
}