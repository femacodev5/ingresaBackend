using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Persons_PersonId1",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_PersonId1",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Persons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PersonId1",
                table: "Persons",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Persons_PersonId1",
                table: "Persons",
                column: "PersonId1",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }
    }
}
