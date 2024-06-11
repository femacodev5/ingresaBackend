using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Personas_PersonaId",
                table: "Asistencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Marcaciones_Personas_PersonaId",
                table: "Marcaciones");

            migrationBuilder.DropIndex(
                name: "IX_Marcaciones_PersonaId",
                table: "Marcaciones");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_PersonaId",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "PersonaId",
                table: "Marcaciones",
                newName: "empID");

            migrationBuilder.RenameColumn(
                name: "PersonaId",
                table: "Asistencias",
                newName: "empID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "empID",
                table: "Marcaciones",
                newName: "PersonaId");

            migrationBuilder.RenameColumn(
                name: "empID",
                table: "Asistencias",
                newName: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcaciones_PersonaId",
                table: "Marcaciones",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_PersonaId",
                table: "Asistencias",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencias_Personas_PersonaId",
                table: "Asistencias",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marcaciones_Personas_PersonaId",
                table: "Marcaciones",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
