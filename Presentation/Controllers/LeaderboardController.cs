using System.Text.Json;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Shared;

namespace Presentation.Controllers
{
    [Route("api/leaderboards")]
    [ApiController]
    public class LeaderboardController: ControllerBase
    {
        private readonly IServiceManager _service;

        public LeaderboardController(IServiceManager service)
        {
            _service = service;
        }
    }
}