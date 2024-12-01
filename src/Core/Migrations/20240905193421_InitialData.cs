using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace streak.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd"), "2 Dev Street", "GER", "Admin_Solutions Ltd" },
                    { new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80"), "1 Dev Street", "GER", "IT_Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Leaderboard",
                columns: new[] { "LeaderboardId", "Name", "OrganizationId" },
                values: new object[,]
                {
                    { new Guid("902ed363-ae11-4b6f-ba59-9f8ba6d08e9b"), "IT Service Sales", new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80") },
                    { new Guid("a478da4c-a47b-4d95-896f-06368e844232"), "Product Sales", new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd") }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "ParticipantId", "Age", "Name", "OrganizationId", "Position" },
                values: new object[,]
                {
                    { new Guid("063a04a0-5e1a-44a0-8005-e1737878e712"), 25, "John Smith", new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd"), "Software developer" },
                    { new Guid("54a31fcb-6d7a-45a4-a60d-e505cec67fa6"), 22, "Jane Hancock", new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80"), "Administrator" },
                    { new Guid("79e49410-c239-4443-bc96-30a515289c97"), 28, "Jane Smith", new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd"), "Software developer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leaderboard",
                keyColumn: "LeaderboardId",
                keyValue: new Guid("902ed363-ae11-4b6f-ba59-9f8ba6d08e9b"));

            migrationBuilder.DeleteData(
                table: "Leaderboard",
                keyColumn: "LeaderboardId",
                keyValue: new Guid("a478da4c-a47b-4d95-896f-06368e844232"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "ParticipantId",
                keyValue: new Guid("063a04a0-5e1a-44a0-8005-e1737878e712"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "ParticipantId",
                keyValue: new Guid("54a31fcb-6d7a-45a4-a60d-e505cec67fa6"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "ParticipantId",
                keyValue: new Guid("79e49410-c239-4443-bc96-30a515289c97"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "OrganizationId",
                keyValue: new Guid("7edac2a8-a73f-4926-8da3-fea7dbaf2ebd"));

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "OrganizationId",
                keyValue: new Guid("c36f337b-2006-4b38-8883-f3c176d9ff80"));
        }
    }
}
