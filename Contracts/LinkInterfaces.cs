using Entities.Models;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Contracts
{
    public interface IParticipantLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<ParticipantDto> participantsDto,
            string fields, Guid orgId, HttpContext httpContext);
    }
}