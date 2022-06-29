using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations; 
public partial class Add_OpeningHoursChange : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "OpeningHoursChanges",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpeningHoursChanges", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_OpeningHoursChanges_Date",
            table: "OpeningHoursChanges",
            column: "Date",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OpeningHoursChanges");
    }
}
