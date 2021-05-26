using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApp.Infrastructure.Migrations
{
    public partial class AddTwoFieldsEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");
        }
    }
}
