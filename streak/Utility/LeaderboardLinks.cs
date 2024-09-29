using Contracts;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared;

namespace streak.Utility
{
    public class LeaderboardLinks: ILeaderboardLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<LeaderboardDto> _dataShaper;

        public LeaderboardLinks(LinkGenerator linkGenerator, IDataShaper<LeaderboardDto> 
        dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        
        public LinkResponse TryGenerateLinks(IEnumerable<LeaderboardDto> leaderboardsDto, string fields, Guid orgId,
            HttpContext httpContext)
        {
            var shapedLeaderboards = ShapeData(leaderboardsDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedLeaderboards(leaderboardsDto, fields, orgId, httpContext,
                    shapedLeaderboards);
            return ReturnShapedLeaderboards(shapedLeaderboards);
        }

        private LinkResponse ReturnLinkedLeaderboards(IEnumerable<LeaderboardDto> leaderboardsDto, string fields, Guid orgId, HttpContext httpContext, List<Entity> shapedLeaderboards)
        {
            var leaderboardDtoList = leaderboardsDto.ToList();
            for (var index = 0; index < leaderboardDtoList.Count(); index++)
            {
                var leaderboardLinks = CreateLinksForLeaderboard(httpContext, orgId,
                    leaderboardDtoList[index].Id, fields);
                shapedLeaderboards[index].Add("Links", leaderboardLinks);
            }
            var leaderboardCollection = new LinkCollectionWrapper<Entity>(shapedLeaderboards);
            var linkedLeaderboards = CreateLinksForLeaderboards(httpContext, leaderboardCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = linkedLeaderboards };
        }

        private List<Link> CreateLinksForLeaderboard(HttpContext httpContext, Guid orgId, Guid
         id, string fields = "")
        {
            var links = new List<Link>
            {
                new(
                    _linkGenerator.GetUriByAction(
                            httpContext, 
                        "GetParticipantForOrganization",
                            controller: "Participants",
                            values: new { orgId, id, fields }
                        ),
                    "self", 
                    "GET"
                    ),
                new(
                    _linkGenerator.GetUriByAction(
                        httpContext, 
                        "DeleteParticipantForOrg",
                        controller: "Participants",
                        values: new { orgId, id }
                    ),
                    "delete_participant", 
                    "DELETE"
                ),
                new(
                    _linkGenerator.GetUriByAction(
                        httpContext, 
                        "UpdateParticipantForOrg",
                        controller: "Participants",
                        values: new { orgId, id }
                    ),
                    "update_participant", 
                    "PUT"
                ),
                new(
                    _linkGenerator.GetUriByAction(
                        httpContext, 
                        "PartiallyUpdateParticipantForOrg",
                        controller: "Participants",
                        values: new { orgId, id }
                    ),
                    "partially_update_participant", 
                    "PATCH"
                )
            };

            return links;
        }
        
        private LinkCollectionWrapper<Entity> CreateLinksForLeaderboards(HttpContext httpContext,
         LinkCollectionWrapper<Entity> leaderboardsWrapper)
        {
            leaderboardsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
                    "GetParticipantsForOrganization", controller: "Participants", values: new { }),
                "self",
                "GET"));
            return leaderboardsWrapper;
        }

        private LinkResponse ReturnShapedLeaderboards(List<Entity> shapedLeaderboards)
        {
            return new LinkResponse { ShapedEntities = shapedLeaderboards };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
                StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<LeaderboardDto> leaderboardsDto, string fields)
        {
            return _dataShaper.ShapeData(leaderboardsDto, fields)
                .Select(e => e.Entity)
                .ToList();
        }
    }
}