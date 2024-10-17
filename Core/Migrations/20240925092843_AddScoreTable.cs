using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace streak.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboard_Organizations_OrganizationId",
                table: "Leaderboard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard");

            migrationBuilder.RenameTable(
                name: "Leaderboard",
                newName: "Leaderboards");

            migrationBuilder.RenameIndex(
                name: "IX_Leaderboard_OrganizationId",
                table: "Leaderboards",
                newName: "IX_Leaderboards_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leaderboards",
                table: "Leaderboards",
                column: "LeaderboardId");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaderboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => new { x.LeaderboardId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_Scores_Leaderboards_LeaderboardId",
                        column: x => x.LeaderboardId,
                        principalTable: "Leaderboards",
                        principalColumn: "LeaderboardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_ParticipantId",
                table: "Scores",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboards_Organizations_OrganizationId",
                table: "Leaderboards",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboards_Organizations_OrganizationId",
                table: "Leaderboards");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leaderboards",
                table: "Leaderboards");

            migrationBuilder.RenameTable(
                name: "Leaderboards",
                newName: "Leaderboard");

            migrationBuilder.RenameIndex(
                name: "IX_Leaderboards_OrganizationId",
                table: "Leaderboard",
                newName: "IX_Leaderboard_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leaderboard",
                table: "Leaderboard",
                column: "LeaderboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboard_Organizations_OrganizationId",
                table: "Leaderboard",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
