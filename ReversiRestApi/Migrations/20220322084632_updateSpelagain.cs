using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ReversiRestApi.Migrations
{
    public partial class updateSpelagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReversiSpellen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speler1Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speler2Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AanDeBeurt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Winnaar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReversiSpellen", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReversiSpellen");
        }
    }
}
