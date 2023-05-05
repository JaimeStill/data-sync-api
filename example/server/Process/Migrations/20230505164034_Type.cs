using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Migrations
{
    /// <inheritdoc />
    public partial class Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Resource",
                newName: "ResourceType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResourceType",
                table: "Resource",
                newName: "Type");
        }
    }
}
