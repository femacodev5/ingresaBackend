using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ingresa.Migrations
{
    /// <inheritdoc />
    public partial class nuevo7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ShiftDetail",
                newName: "InicioMarcacionSalida");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "ShiftDetail",
                newName: "InicioMarcacionInicioDescanso");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FinMarcacionDescanso",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FinMarcacionEntrada",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FinMarcacionSalida",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "HabilitarDescanso",
                table: "ShiftDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraIngreso",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraSalida",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "InicioMarcacionEntrada",
                table: "ShiftDetail",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "MinutosDescanso",
                table: "ShiftDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinMarcacionDescanso",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "FinMarcacionEntrada",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "FinMarcacionSalida",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "HabilitarDescanso",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "HoraIngreso",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "HoraSalida",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "InicioMarcacionEntrada",
                table: "ShiftDetail");

            migrationBuilder.DropColumn(
                name: "MinutosDescanso",
                table: "ShiftDetail");

            migrationBuilder.RenameColumn(
                name: "InicioMarcacionSalida",
                table: "ShiftDetail",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "InicioMarcacionInicioDescanso",
                table: "ShiftDetail",
                newName: "EndTime");
        }
    }
}
