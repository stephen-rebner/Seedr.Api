using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Seedr.Api.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddEnvironments : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "environments",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_environments", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "environments");
    }
}
