using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations; 
public partial class Add_User_Mailing : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<bool>(
            name: "SendNews",
            table: "AspNetUsers",
            type: "bit",
            nullable: false,
            defaultValue: true);

        migrationBuilder.AddColumn<bool>(
            name: "SendNotifications",
            table: "AspNetUsers",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropColumn(
            name: "SendNews",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "SendNotifications",
            table: "AspNetUsers");
    }
}
