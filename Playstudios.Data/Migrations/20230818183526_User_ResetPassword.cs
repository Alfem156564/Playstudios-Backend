using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playstudios.Data.Migrations
{
    public partial class User_ResetPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCode",
                schema: "Account",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCode",
                schema: "Account",
                table: "User");
        }
    }
}
