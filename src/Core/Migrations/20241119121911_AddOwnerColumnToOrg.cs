using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace streak.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerColumnToOrg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Organizations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AspNetUsers_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AspNetUsers_OwnerId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Organizations");
        }
    }
}
