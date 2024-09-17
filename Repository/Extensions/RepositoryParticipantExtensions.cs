using System.Reflection;
using System.Text;
using Entities;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryParticipantExtensions
    {
        public static IQueryable<Participant> FilterParticipants(this IQueryable<Participant>
            participants, uint minAge, uint maxAge) =>
            participants.Where(e => (e.Age >= minAge && e.Age <= maxAge));

        public static IQueryable<Participant> Search(this IQueryable<Participant> participants,
            string? searchTerm)
        {
            return string.IsNullOrWhiteSpace(searchTerm) ? participants : participants.Where(e => e.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        public static IQueryable<Participant> Sort(this IQueryable<Participant> participants,
            string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return participants.OrderBy(e => e.Name);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderByQuery<Participant>(orderByQueryString);

            return string.IsNullOrWhiteSpace(orderQuery) ? participants.OrderBy(e => e.Name) : participants.OrderBy(orderQuery);
        }
    }
}