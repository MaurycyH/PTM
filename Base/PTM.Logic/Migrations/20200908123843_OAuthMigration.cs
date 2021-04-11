using Microsoft.EntityFrameworkCore.Migrations;

namespace PTM.Logic.Migrations
{
    public partial class OAuthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OAuthID",
                table: "User",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OAuthID",
                table: "User");
        }
    }
}
