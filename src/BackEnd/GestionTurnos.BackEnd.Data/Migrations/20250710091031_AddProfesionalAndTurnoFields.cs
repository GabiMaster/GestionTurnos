using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTurnos.BackEnd.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfesionalAndTurnoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaExpiracionQR",
                table: "Turnos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfesionalId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Profesionales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesionales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicioProfesional",
                columns: table => new
                {
                    ProfesionalesId = table.Column<int>(type: "int", nullable: false),
                    ServiciosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicioProfesional", x => new { x.ProfesionalesId, x.ServiciosId });
                    table.ForeignKey(
                        name: "FK_ServicioProfesional_Profesionales_ProfesionalesId",
                        column: x => x.ProfesionalesId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicioProfesional_Servicios_ServiciosId",
                        column: x => x.ServiciosId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_ProfesionalId",
                table: "Turnos",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicioProfesional_ServiciosId",
                table: "ServicioProfesional",
                column: "ServiciosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Profesionales_ProfesionalId",
                table: "Turnos",
                column: "ProfesionalId",
                principalTable: "Profesionales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Profesionales_ProfesionalId",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "ServicioProfesional");

            migrationBuilder.DropTable(
                name: "Profesionales");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_ProfesionalId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "FechaExpiracionQR",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "ProfesionalId",
                table: "Turnos");
        }
    }
}
