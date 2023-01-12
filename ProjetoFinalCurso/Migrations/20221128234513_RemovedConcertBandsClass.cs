using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoFinalCurso.Migrations
{
    public partial class RemovedConcertBandsClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcertBands");

            migrationBuilder.AddColumn<int>(
                name: "ConcertId",
                table: "Bands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bands_ConcertId",
                table: "Bands",
                column: "ConcertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bands_Concerts_ConcertId",
                table: "Bands",
                column: "ConcertId",
                principalTable: "Concerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bands_Concerts_ConcertId",
                table: "Bands");

            migrationBuilder.DropIndex(
                name: "IX_Bands_ConcertId",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "ConcertId",
                table: "Bands");

            migrationBuilder.CreateTable(
                name: "ConcertBands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandaId = table.Column<int>(type: "int", nullable: true),
                    ConcertoId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcertBands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcertBands_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcertBands_Bands_BandaId",
                        column: x => x.BandaId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcertBands_Concerts_ConcertoId",
                        column: x => x.ConcertoId,
                        principalTable: "Concerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcertBands_BandaId",
                table: "ConcertBands",
                column: "BandaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcertBands_ConcertoId",
                table: "ConcertBands",
                column: "ConcertoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcertBands_UserId",
                table: "ConcertBands",
                column: "UserId");
        }
    }
}
