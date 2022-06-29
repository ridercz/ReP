using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations; 
public partial class Add_NewsMessage : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "NewsMessages",
            columns: table => new {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_NewsMessages", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "NewsMessages");
    }
}
