using Microsoft.AspNetCore.Http;
using Shared;

namespace Entities
{
    public class Link
    {
        public Link()
        {
        }

        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Method { get; set; }
    }

    public class LinkResourceBase
    {
        public List<Link> Links { get; set; } = new();
    }

    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public LinkCollectionWrapper()
        {
        }

        public LinkCollectionWrapper(List<T> value)
        {
            Value = value;
        }

        public List<T> Value { get; set; } = new();
    }

    public class LinkResponse
    {
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<Entity>();
            ShapedEntities = new List<Entity>();
        }

        public bool HasLinks { get; set; }
        public List<Entity> ShapedEntities { get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }
    }

    public record LinkParameters(ParticipantParameters ParticipantParameters, HttpContext Context);

    public record ScoreLinkParams(ScoreParameters ScoreParameters, HttpContext Context);

    public record LeaderboardLinkParams(
        LeaderboardParameters LeaderboardParameters,
        HttpContext
            Context);
}