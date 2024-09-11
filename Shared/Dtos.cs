namespace Shared;

public record OrganizationDto(Guid Id, string Name, string FullAddress);

public record ParticipantDto(Guid Id, string Name, int Age, string Position);