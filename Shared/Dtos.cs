namespace Shared;

// [Serializable]
public record OrganizationDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? FullAddress { get; init; }
}

public record OrgForCreationDto(string Name, string Address, string Country, 
IEnumerable<ParticipantForCreationDto> Participants);

public record ParticipantDto(Guid Id, string Name, int Age, string Position);

public record ParticipantForCreationDto(string Name, int Age, string Position);