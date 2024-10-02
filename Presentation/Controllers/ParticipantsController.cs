using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/participants")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ParticipantsController(IServiceManager service)
        {
            _service = service;
        }

        
    }
}