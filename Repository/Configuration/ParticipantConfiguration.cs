using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasData(
                new Participant
                {
                    Id = new Guid("063a04a0-5e1a-44a0-8005-e1737878e712"),
                    Name = "John Smith",
                    Age = 25,
                    Position = "Software developer",
                    OrganizationId = new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd")
                },
                new Participant
                {
                    Id = new Guid("79e49410-c239-4443-bc96-30a515289c97"),
                    Name = "Jane Smith",
                    Age = 28,
                    Position = "Software developer",
                    OrganizationId = new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd")
                },
                new Participant
                {
                    Id = new Guid("54a31fcb-6d7a-45a4-a60d-e505cec67fa6"),
                    Name = "Jane Hancock",
                    Age = 22,
                    Position = "Administrator",
                    OrganizationId = new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80")
                }
            );
        }
    }
}