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

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos =
                typeof(Participant).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propFromQueryName = param.Split(" ")[0];
                var objProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals
                    (propFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objProperty.Name} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return participants.OrderBy(e => e.Name);
            }

            return participants.OrderBy(orderQuery);
        }
    }
}