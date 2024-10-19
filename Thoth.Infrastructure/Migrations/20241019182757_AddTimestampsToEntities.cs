using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "thoth",
                table: "user_roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "user_roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "user_roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "thoth",
                table: "role_permissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "role_permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "role_permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "thoth",
                table: "organizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_at",
                schema: "thoth",
                table: "organizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "thoth",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "thoth",
                table: "role_permissions");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "role_permissions");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "role_permissions");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "thoth",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "modified_at",
                schema: "thoth",
                table: "organizations");
        }
    }
}
