using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.SqlServer {
    public partial class Add_Resource_Attachments_Table : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAttachment_Resources_ResourceId",
                table: "ResourceAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceAttachment",
                table: "ResourceAttachment");

            migrationBuilder.RenameTable(
                name: "ResourceAttachment",
                newName: "ResourceAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_ResourceAttachment_ResourceId",
                table: "ResourceAttachments",
                newName: "IX_ResourceAttachments_ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceAttachments",
                table: "ResourceAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAttachments_Resources_ResourceId",
                table: "ResourceAttachments",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAttachments_Resources_ResourceId",
                table: "ResourceAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceAttachments",
                table: "ResourceAttachments");

            migrationBuilder.RenameTable(
                name: "ResourceAttachments",
                newName: "ResourceAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_ResourceAttachments_ResourceId",
                table: "ResourceAttachment",
                newName: "IX_ResourceAttachment_ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceAttachment",
                table: "ResourceAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAttachment_Resources_ResourceId",
                table: "ResourceAttachment",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
