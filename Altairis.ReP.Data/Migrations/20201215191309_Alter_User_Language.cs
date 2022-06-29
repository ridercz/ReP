using Microsoft.EntityFrameworkCore.Migrations;

namespace Altairis.ReP.Data.Migrations;
public partial class Alter_User_Language : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "AspNetUsers",
            type: "nchar(5)",
            fixedLength: true,
            maxLength: 5,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nchar(2)",
            oldFixedLength: true,
            oldMaxLength: 2);
        migrationBuilder.Sql("UPDATE AspNetUsers SET Language='cs-CZ' WHERE Language='cs'");
        migrationBuilder.Sql("UPDATE AspNetUsers SET Language='en-US' WHERE Language='en'");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "AspNetUsers",
            type: "nchar(2)",
            fixedLength: true,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nchar(5)",
            oldFixedLength: true,
            oldMaxLength: 5);
    }
}
