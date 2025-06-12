using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlDbApplication.Migrations
{
    public partial class CreatedOnInCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PrivateEndpointMetadatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorMsg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateEndpointMetadatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateEndpointReferenceMetadatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateEndpointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateEndpointReferenceMetadatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateEndpointReferenceMetadatas_PrivateEndpointMetadatas_PrivateEndpointId",
                        column: x => x.PrivateEndpointId,
                        principalTable: "PrivateEndpointMetadatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateEndpointTargetResourceMetadatas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateEndpointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateEndpointTargetResourceMetadatas", x => x.id);
                    table.ForeignKey(
                        name: "FK_PrivateEndpointTargetResourceMetadatas_PrivateEndpointMetadatas_PrivateEndpointId",
                        column: x => x.PrivateEndpointId,
                        principalTable: "PrivateEndpointMetadatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateEndpointReferenceMetadatas_PrivateEndpointId",
                table: "PrivateEndpointReferenceMetadatas",
                column: "PrivateEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateEndpointTargetResourceMetadatas_PrivateEndpointId",
                table: "PrivateEndpointTargetResourceMetadatas",
                column: "PrivateEndpointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateEndpointReferenceMetadatas");

            migrationBuilder.DropTable(
                name: "PrivateEndpointTargetResourceMetadatas");

            migrationBuilder.DropTable(
                name: "PrivateEndpointMetadatas");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Cities");
        }
    }
}
