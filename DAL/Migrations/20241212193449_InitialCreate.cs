using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigName = table.Column<string>(type: "TEXT", nullable: false),
                    BoardSize = table.Column<int>(type: "INTEGER", nullable: false),
                    GridSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PiecesAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerOnePiece = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerTwoPiece = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameName = table.Column<string>(type: "TEXT", nullable: false),
                    GridTopLeft = table.Column<string>(type: "TEXT", nullable: false),
                    PositionsJson = table.Column<string>(type: "TEXT", nullable: false),
                    ConfigId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstPlayerPassword = table.Column<string>(type: "TEXT", nullable: false),
                    SecondPlayerPassword = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedGames_Configurations_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedGames_ConfigId",
                table: "SavedGames",
                column: "ConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedGames");

            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
