using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasData(
                new Organization
                {
                    Id = new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80"),
                    Name = "IT_Solutions Ltd",
                    Address = "1 Dev Street",
                    Country = "GER",
                    OwnerId = "18e62a3b-f503-4d2f-9b22-9f7a6f0ede10"
                },
                new Organization
                {
                    Id = new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd"),
                    Name = "Admin_Solutions Ltd",
                    Address = "2 Dev Street",
                    Country = "GER",
                    OwnerId = "18e62a3b-f503-4d2f-9b22-9f7a6f0ede10"
                }
            );
        }
    }
}