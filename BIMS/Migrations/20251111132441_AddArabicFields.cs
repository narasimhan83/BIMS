using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "DocumentTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "DocumentTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "CustomerTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "CustomerTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "BusinessTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "BusinessTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DescriptionAr", "NameAr" },
                values: new object[] { new DateTime(2025, 11, 11, 13, 24, 40, 759, DateTimeKind.Utc).AddTicks(6058), null, null });

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DescriptionAr", "NameAr" },
                values: new object[] { new DateTime(2025, 11, 11, 13, 24, 40, 759, DateTimeKind.Utc).AddTicks(6059), null, null });

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "DescriptionAr", "NameAr" },
                values: new object[] { new DateTime(2025, 11, 11, 13, 24, 40, 759, DateTimeKind.Utc).AddTicks(6061), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "CustomerTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "BusinessTypes");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 11, 12, 50, 33, 683, DateTimeKind.Utc).AddTicks(6676));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 11, 12, 50, 33, 683, DateTimeKind.Utc).AddTicks(6678));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 11, 12, 50, 33, 683, DateTimeKind.Utc).AddTicks(6679));
        }
    }
}
