using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FsElements.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_532 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActiveSeller",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveSeller",
                table: "AspNetUsers");
        }
    }
}
