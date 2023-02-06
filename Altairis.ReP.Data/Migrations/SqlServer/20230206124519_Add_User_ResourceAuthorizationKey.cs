using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altairis.ReP.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddUserResourceAuthorizationKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceAuthorizationKey",
                table: "AspNetUsers",
                type: "nchar(30)",
                fixedLength: true,
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceAuthorizationKey",
                table: "AspNetUsers");
        }
    }
}
