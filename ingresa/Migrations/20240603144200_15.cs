using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ShiftId",
                table: "Persons",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Shift_ShiftId",
                table: "Persons",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Shift_ShiftId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_ShiftId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Persons");
        }
    }
}
