using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Leaderboard
    {
        [Column("LeaderboardId")] public Guid Id { get; set; }

        [Required(ErrorMessage = "Leaderboard name is a required field")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters")]
        public string? Name { get; set; }

        [ForeignKey(nameof(Organization))] public Guid OrganizationId { get; set; }

        public Organization? Organization { get; set; }

        public List<Score> Scores { get; set; } = new();
        public List<Participant> Participants { get; set; } = new();
    }
}