using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConversationsEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Conversations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 11, 11, 37, 21, 932, DateTimeKind.Utc).AddTicks(592),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 4, 19, 32, 53, 288, DateTimeKind.Utc).AddTicks(7106));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 4, 19, 32, 53, 288, DateTimeKind.Utc).AddTicks(7106),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 11, 11, 37, 21, 932, DateTimeKind.Utc).AddTicks(592));
        }
    }
}
