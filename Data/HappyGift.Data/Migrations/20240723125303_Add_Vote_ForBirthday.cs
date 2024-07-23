using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyGift.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Vote_ForBirthday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Votes",
                newName: "ForUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                newName: "IX_Votes_ForUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ForBirthdayDate",
                table: "Votes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_ForUserId",
                table: "Votes",
                column: "ForUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_ForUserId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "ForBirthdayDate",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "ForUserId",
                table: "Votes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_ForUserId",
                table: "Votes",
                newName: "IX_Votes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
