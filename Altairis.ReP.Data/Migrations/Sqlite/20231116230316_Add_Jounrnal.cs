using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.Sqlite; 
/// <inheritdoc />
public partial class Add_Jounrnal : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "JournalEntries",
            columns: table => new {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                ResourceId = table.Column<int>(type: "INTEGER", nullable: true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Text = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_JournalEntries", x => x.Id);
                table.ForeignKey(
                    name: "FK_JournalEntries_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_JournalEntries_Resources_ResourceId",
                    column: x => x.ResourceId,
                    principalTable: "Resources",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "JournalEntryAttachments",
            columns: table => new {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                JournalEntryId = table.Column<int>(type: "INTEGER", nullable: false),
                FileName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                StoragePath = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_JournalEntryAttachments", x => x.Id);
                table.ForeignKey(
                    name: "FK_JournalEntryAttachments_JournalEntries_JournalEntryId",
                    column: x => x.JournalEntryId,
                    principalTable: "JournalEntries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_JournalEntries_ResourceId",
            table: "JournalEntries",
            column: "ResourceId");

        migrationBuilder.CreateIndex(
            name: "IX_JournalEntries_UserId",
            table: "JournalEntries",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_JournalEntryAttachments_JournalEntryId",
            table: "JournalEntryAttachments",
            column: "JournalEntryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "JournalEntryAttachments");

        migrationBuilder.DropTable(
            name: "JournalEntries");
    }
}
