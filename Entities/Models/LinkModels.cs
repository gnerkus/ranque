using Microsoft.AspNetCore.Http;
using Shared;

namespace Entities.Models
{
    public class Link
    {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Method { get; set; }

        public Link()
        {
            
        }

        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
    
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {
            
        }

        public List<Link> Links { get; set; } = new List<Link>();
    }

    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public List<T> Value { get; set; } = new List<T>();

        public LinkCollectionWrapper()
        {
            
        }

        public LinkCollectionWrapper(List<T> value) => Value = value;
    }

    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Entity> ShapedEntities { get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<Entity>();
            ShapedEntities = new List<Entity>();
        }
    }

    public record LinkParameters(ParticipantParameters ParticipantParameters, HttpContext Context);

    public record ScoreLinkParams(ScoreParameters ScoreParameters, HttpContext Context);
}