using Microsoft.EntityFrameworkCore.Migrations;

namespace SqlDbApplication.Migrations
{
    public partial class RemovedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
