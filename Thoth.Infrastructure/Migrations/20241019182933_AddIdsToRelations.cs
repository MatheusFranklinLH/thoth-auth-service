using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdsToRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "thoth",
                table: "user_roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "thoth",
                table: "role_permissions",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "thoth",
                table: "user_roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "thoth",
                table: "role_permissions",
                newName: "Id");
        }
    }
}
