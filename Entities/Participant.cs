using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Participant
    {
        [Column("ParticipantId")] public Guid Id { get; set; }

        [Required(ErrorMessage = "Participant name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Age is a required field")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required field")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string? Position { get; set; }

        [ForeignKey(nameof(Organization))] public Guid OrganizationId { get; set; }

        public Organization? Organization { get; set; }

        public List<Score> Scores { get; set; } = new ();
        public List<Leaderboard> Leaderboards { get; set; } = new();
    }
}
