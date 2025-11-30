using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEngineCapacityRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "EngineCapacities",
                newName: "CapacityTo");

            migrationBuilder.AddColumn<decimal>(
                name: "CapacityFrom",
                table: "EngineCapacities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 15, 23, 7, 855, DateTimeKind.Utc).AddTicks(2587));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 15, 23, 7, 855, DateTimeKind.Utc).AddTicks(2590));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 15, 23, 7, 855, DateTimeKind.Utc).AddTicks(2592));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapacityFrom",
                table: "EngineCapacities");

            migrationBuilder.RenameColumn(
                name: "CapacityTo",
                table: "EngineCapacities",
                newName: "Capacity");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 24, 8, 54, 55, 385, DateTimeKind.Utc).AddTicks(4074));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 24, 8, 54, 55, 385, DateTimeKind.Utc).AddTicks(4077));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 24, 8, 54, 55, 385, DateTimeKind.Utc).AddTicks(4079));
        }
    }
}
