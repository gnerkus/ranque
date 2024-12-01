using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace streak.Migrations
{
    /// <inheritdoc />
    public partial class AddLuaScriptColToLeaderboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LuaScript",
                table: "Leaderboards",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Leaderboards",
                keyColumn: "LeaderboardId",
                keyValue: new Guid("902ed363-ae11-4b6f-ba59-9f8ba6d08e9b"),
                column: "LuaScript",
                value: "return score.First + score.Second");

            migrationBuilder.UpdateData(
                table: "Leaderboards",
                keyColumn: "LeaderboardId",
                keyValue: new Guid("a478da4c-a47b-4d95-896f-06368e844232"),
                column: "LuaScript",
                value: "return score.First + score.Second");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LuaScript",
                table: "Leaderboards");
        }
    }
}
