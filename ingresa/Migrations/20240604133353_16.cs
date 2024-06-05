using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class _16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Shift_ShiftId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDetail_Shift_ShiftId",
                table: "ShiftDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDetail",
                table: "ShiftDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.RenameTable(
                name: "ShiftDetail",
                newName: "ShiftDetails");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDetail_ShiftId",
                table: "ShiftDetails",
                newName: "IX_ShiftDetails_ShiftId");

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDetails",
                table: "ShiftDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "ShiftId");

            migrationBuilder.CreateTable(
                name: "DialingRecords",
                columns: table => new
                {
                    DialingRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialingRecords", x => x.DialingRecordId);
                    table.ForeignKey(
                        name: "FK_DialingRecords_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PersonId1",
                table: "Persons",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_DialingRecords_PersonId",
                table: "DialingRecords",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Persons_PersonId1",
                table: "Persons",
                column: "PersonId1",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Shifts_ShiftId",
                table: "Persons",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDetails_Shifts_ShiftId",
                table: "ShiftDetails",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Persons_PersonId1",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Shifts_ShiftId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDetails_Shifts_ShiftId",
                table: "ShiftDetails");

            migrationBuilder.DropTable(
                name: "DialingRecords");

            migrationBuilder.DropIndex(
                name: "IX_Persons_PersonId1",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDetails",
                table: "ShiftDetails");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameTable(
                name: "ShiftDetails",
                newName: "ShiftDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDetails_ShiftId",
                table: "ShiftDetail",
                newName: "IX_ShiftDetail_ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDetail",
                table: "ShiftDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Shift_ShiftId",
                table: "Persons",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDetail_Shift_ShiftId",
                table: "ShiftDetail",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
