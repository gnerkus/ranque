using Entities;

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
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return participants;
            }

            return participants.Where(e => e.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }
    }
}