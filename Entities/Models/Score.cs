using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Score
    {
        public Guid Id { get; init; }

        // TODO: add custom validation for JSONValue to ensure string is valid JSON 
        [Required(ErrorMessage = "Score must have a value")]
        [MaxLength(1000, ErrorMessage = "Maximum length for the JSONValue is 30 characters")]
        public required string JsonValue { get; set; }

        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(Participant))] public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; } = null!;

        [ForeignKey(nameof(Leaderboard))] public Guid LeaderboardId { get; set; }
        public Leaderboard Leaderboard { get; set; } = null!;
    }
}