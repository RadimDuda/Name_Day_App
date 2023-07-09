using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NameDayWorker.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NameDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Names",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    NameDayId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Names", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Names_NameDays_NameDayId",
                        column: x => x.NameDayId,
                        principalTable: "NameDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Names_NameDayId",
                table: "Names",
                column: "NameDayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Names");

            migrationBuilder.DropTable(
                name: "NameDays");
        }
    }
}
