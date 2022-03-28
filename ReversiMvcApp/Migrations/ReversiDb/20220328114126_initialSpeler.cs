using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiMvcApp.Migrations.ReversiDb
{
    public partial class initialSpeler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GebruikerRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikerRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AantalGewonnen = table.Column<int>(type: "int", nullable: false),
                    AantalVerloren = table.Column<int>(type: "int", nullable: false),
                    AantalGelijk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GebruikerRole");

            migrationBuilder.DropTable(
                name: "Spelers");
        }
    }
}
