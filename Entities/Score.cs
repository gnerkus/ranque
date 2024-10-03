using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Score
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Score must have a value")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public float Value { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(Participant))] public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; } = null!;

        [ForeignKey(nameof(Leaderboard))] public Guid LeaderboardId { get; set; }
        public Leaderboard Leaderboard { get; set; } = null!;
    }
}