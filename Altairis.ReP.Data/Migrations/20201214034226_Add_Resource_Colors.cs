using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations;
public partial class Add_Resource_Colors : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<string>(
            name: "BackgroundColor",
            table: "Resources",
            type: "nchar(7)",
            fixedLength: true,
            maxLength: 7,
            nullable: false,
            defaultValue: "#ffffff");

        migrationBuilder.AddColumn<string>(
            name: "ForegroundColor",
            table: "Resources",
            type: "nchar(7)",
            fixedLength: true,
            maxLength: 7,
            nullable: false,
            defaultValue: "#000000");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropColumn(
            name: "BackgroundColor",
            table: "Resources");

        migrationBuilder.DropColumn(
            name: "ForegroundColor",
            table: "Resources");
    }
}
