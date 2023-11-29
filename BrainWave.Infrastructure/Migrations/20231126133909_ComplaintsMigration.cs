using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Articles_ArticleId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_StatusComplaints_StatusId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ArticleId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Complaints",
                newName: "ArticleComplaintId");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_StatusId",
                table: "Complaints",
                newName: "IX_Complaints_ArticleComplaintId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 26, 13, 39, 8, 961, DateTimeKind.Utc).AddTicks(9807),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 21, 15, 23, 2, 324, DateTimeKind.Utc).AddTicks(3168));

            migrationBuilder.CreateTable(
                name: "ArticleComplaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComplaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComplaints_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArticleComplaints_StatusComplaints_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusComplaints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComplaints_ArticleId",
                table: "ArticleComplaints",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComplaints_StatusId",
                table: "ArticleComplaints",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_ArticleComplaints_ArticleComplaintId",
                table: "Complaints",
                column: "ArticleComplaintId",
                principalTable: "ArticleComplaints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_ArticleComplaints_ArticleComplaintId",
                table: "Complaints");

            migrationBuilder.DropTable(
                name: "ArticleComplaints");

            migrationBuilder.RenameColumn(
                name: "ArticleComplaintId",
                table: "Complaints",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_ArticleComplaintId",
                table: "Complaints",
                newName: "IX_Complaints_StatusId");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 21, 15, 23, 2, 324, DateTimeKind.Utc).AddTicks(3168),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 26, 13, 39, 8, 961, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ArticleId",
                table: "Complaints",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Articles_ArticleId",
                table: "Complaints",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_StatusComplaints_StatusId",
                table: "Complaints",
                column: "StatusId",
                principalTable: "StatusComplaints",
                principalColumn: "Id");
        }
    }
}
