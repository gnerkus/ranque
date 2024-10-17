using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Organization
    {
        [Column("OrganizationId")] public Guid Id { get; set; }

        [Required(ErrorMessage = "Organization name is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Company address is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string? Address { get; set; }

        public string? Country { get; set; }

        public List<Leaderboard> Leaderboards { get; set; } = new();
        public List<Participant> Participants { get; set; } = new();
    }
}