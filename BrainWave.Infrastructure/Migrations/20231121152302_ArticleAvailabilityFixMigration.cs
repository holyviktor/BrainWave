using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ArticleAvailabilityFixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 21, 15, 23, 2, 324, DateTimeKind.Utc).AddTicks(3168),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 21, 15, 21, 47, 294, DateTimeKind.Utc).AddTicks(8171));

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 21, 15, 21, 47, 294, DateTimeKind.Utc).AddTicks(8171),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 21, 15, 23, 2, 324, DateTimeKind.Utc).AddTicks(3168));

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Articles",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
