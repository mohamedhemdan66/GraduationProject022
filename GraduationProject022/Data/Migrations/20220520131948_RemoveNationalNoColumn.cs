using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject022.Data.Migrations
{
    public partial class RemoveNationalNoColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalNo",
                table: "Profile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalNo",
                table: "Profile",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }
    }
}
