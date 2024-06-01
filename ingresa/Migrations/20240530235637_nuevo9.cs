using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class nuevo9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InicioMarcacionInicioDescanso",
                table: "ShiftDetail",
                newName: "InicioMarcacionDescanso");

            migrationBuilder.RenameColumn(
                name: "HoraIngreso",
                table: "ShiftDetail",
                newName: "HoraEntrada");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InicioMarcacionDescanso",
                table: "ShiftDetail",
                newName: "InicioMarcacionInicioDescanso");

            migrationBuilder.RenameColumn(
                name: "HoraEntrada",
                table: "ShiftDetail",
                newName: "HoraIngreso");
        }
    }
}
