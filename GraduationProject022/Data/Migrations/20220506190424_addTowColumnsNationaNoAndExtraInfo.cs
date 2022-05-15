using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject022.Data.Migrations
{
    public partial class addTowColumnsNationaNoAndExtraInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraInfo",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalNo",
                table: "Profile",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraInfo",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "NationalNo",
                table: "Profile");
        }
    }
}
