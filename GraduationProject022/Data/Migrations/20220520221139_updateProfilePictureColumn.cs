using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject022.Data.Migrations
{
    public partial class updateProfilePictureColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Profile");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Profile");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Profile",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
