using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations
{
    public partial class Rename_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "DirectoryEntries",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "DirectoryEntries",
                newName: "EmailAddress");
        }
    }
}
