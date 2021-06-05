using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApp.Infrastructure.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Products",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Categories",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "Picture");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Categories",
                newName: "Picture");
        }
    }
}
