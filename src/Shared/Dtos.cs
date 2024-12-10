using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public record OrganizationDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }

    public record OrgForCreationDto(
        string Name,
        string Address,
        string Country,
        IEnumerable<ParticipantForCreationDto> Participants);

    public record OrgForUpdateDto(
        string Name,
        string Address,
        string Country,
        IEnumerable<ParticipantForCreationDto> Participants);

    public record ScoreDto
    {
        public Guid Id { get; init; }
        public required string JsonValue { get; init; }
        public required ParticipantDto Participant { get; init; }
        public required LeaderboardDto Leaderboard { get; init; }
    }

    public abstract record ScoreForManipulationDto
    {
        [Required(ErrorMessage = "Score must have a value")]
        [MaxLength(1000, ErrorMessage = "Maximum length for the JSONValue is 30 characters")]
        public required string JsonValue { get; init; }
    }

    public record ScoreForCreationDto : ScoreForManipulationDto
    {
        [Required(ErrorMessage = "Leaderboard id is required")]
        public Guid LeaderboardId { get; init; }

        [Required(ErrorMessage = "Participant id is required")]
        public Guid ParticipantId { get; init; }
    }

    public record ScoreForUpdateDto : ScoreForManipulationDto;

    public record LeaderboardDto(Guid Id, string Name, string LuaScript);

    public record RankedLeaderboardDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }

        public required string LuaScript { get; init; }
        public IEnumerable<RankedParticipantDto> Participants { get; init; }
    }

    public abstract record LeaderboardForManipulationDto
    {
        [Required(ErrorMessage = "Leaderboard name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; init; }

        [MaxLength(1000, ErrorMessage = "Maximum length for the Lua script is 1000 characters")]
        public required string LuaScript { get; set; }
    }

    public record LeaderboardForCreationDto : LeaderboardForManipulationDto;

    public record LeaderboardForUpdateDto : LeaderboardForManipulationDto;

    public record ParticipantDto(Guid Id, string Name, int Age, string Position);

    public record RankedParticipantDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public double Score { get; init; }
    }

    public abstract record ParticipantForManipulationDto
    {
        [Required(ErrorMessage = "Participant name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; init; }

        [Range(18, int.MaxValue, ErrorMessage = "Age is required and can't be lower than 18")]
        public int Age { get; init; }

        [Required(ErrorMessage = "Position is a required field")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20")]
        public string? Position { get; init; }
    }

    public record ParticipantForCreationDto : ParticipantForManipulationDto;

    public record ParticipantForUpdateDto : ParticipantForManipulationDto;


    // AUTH =====================================================================
    public record UserForRegistrationDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }

        [Required(ErrorMessage = "Email address is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public ICollection<string>? Roles { get; init; }
    }

    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email address is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password name is required")]
        public string? Password { get; init; }
    }

    public record TokenDto(string AccessToken, string RefreshToken, string? Username);
}