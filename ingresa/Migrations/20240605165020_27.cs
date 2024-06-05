using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonaId",
                table: "Asistencias",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Personas_PersonaId",
                table: "Asistencias");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_PersonaId",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "PersonaId",
                table: "Asistencias");
        }
    }
}
