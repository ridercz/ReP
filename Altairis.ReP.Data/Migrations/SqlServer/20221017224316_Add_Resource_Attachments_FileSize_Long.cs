using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.SqlServer; 
public partial class Add_Resource_Attachments_FileSize_Long : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AlterColumn<long>(
            name: "FileSize",
            table: "ResourceAttachments",
            type: "bigint",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.AlterColumn<int>(
            name: "FileSize",
            table: "ResourceAttachments",
            type: "int",
            nullable: false,
            oldClrType: typeof(long),
            oldType: "bigint");
    }
}
