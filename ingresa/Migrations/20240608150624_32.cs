using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_Personas_PersonaId",
                table: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_PersonaId",
                table: "Contratos");

            migrationBuilder.RenameColumn(
                name: "PersonaId",
                table: "Contratos",
                newName: "EmpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmpID",
                table: "Contratos",
                newName: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_PersonaId",
                table: "Contratos",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_Personas_PersonaId",
                table: "Contratos",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
