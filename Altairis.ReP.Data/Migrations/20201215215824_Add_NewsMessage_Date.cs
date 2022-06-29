using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations;
public partial class Add_NewsMessage_Date : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<DateTime>(
            name: "Date",
            table: "NewsMessages",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropColumn(
            name: "Date",
            table: "NewsMessages");
    }
}
