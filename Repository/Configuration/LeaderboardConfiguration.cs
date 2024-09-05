using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class LeaderboardConfiguration: IEntityTypeConfiguration<Leaderboard>
    {
        public void Configure(EntityTypeBuilder<Leaderboard> builder)
        {
            builder.HasData(
                new Leaderboard
                {
                    Id = new Guid("902ed363-ae11-4b6f-ba59-9f8ba6d08e9b"),
                    Name = "IT Service Sales",
                    OrganizationId = new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80")
                },
                new Leaderboard
                {
                    Id = new Guid("a478da4c-a47b-4d95-896f-06368e844232"),
                    Name = "Product Sales",
                    OrganizationId = new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd")
                }
            );
        }
    }
}