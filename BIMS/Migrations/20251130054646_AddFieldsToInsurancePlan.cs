using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToInsurancePlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LaunchDate",
                table: "InsurancePlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanCode",
                table: "InsurancePlans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanTier",
                table: "InsurancePlans",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithdrawDate",
                table: "InsurancePlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 5, 46, 45, 117, DateTimeKind.Utc).AddTicks(3325));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 5, 46, 45, 117, DateTimeKind.Utc).AddTicks(3329));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 5, 46, 45, 117, DateTimeKind.Utc).AddTicks(3330));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaunchDate",
                table: "InsurancePlans");

            migrationBuilder.DropColumn(
                name: "PlanCode",
                table: "InsurancePlans");

            migrationBuilder.DropColumn(
                name: "PlanTier",
                table: "InsurancePlans");

            migrationBuilder.DropColumn(
                name: "WithdrawDate",
                table: "InsurancePlans");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 20, 42, 42, 856, DateTimeKind.Utc).AddTicks(7027));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 20, 42, 42, 856, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 29, 20, 42, 42, 856, DateTimeKind.Utc).AddTicks(7031));
        }
    }
}
