using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.SqlServer
{
    public partial class Add_Resource_Instructions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Resources");
        }
    }
}
