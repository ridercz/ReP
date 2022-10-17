using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.SqlServer {
    public partial class Add_Resource_Attachments : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "ResourceAttachment",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ResourceAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceAttachment_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAttachment_ResourceId",
                table: "ResourceAttachment",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "ResourceAttachment");
        }
    }
}
