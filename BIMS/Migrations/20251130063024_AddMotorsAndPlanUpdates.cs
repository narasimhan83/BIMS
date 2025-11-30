using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorsAndPlanUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComprehensiveTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePlanId = table.Column<int>(type: "int", nullable: false),
                    ValueBandId = table.Column<int>(type: "int", nullable: false),
                    VehicleCategoryId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Excess = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprehensiveTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprehensiveTariffs_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprehensiveTariffs_ValueBands_ValueBandId",
                        column: x => x.ValueBandId,
                        principalTable: "ValueBands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprehensiveTariffs_VehicleCategories_VehicleCategoryId",
                        column: x => x.VehicleCategoryId,
                        principalTable: "VehicleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprehensiveTariffs_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 6, 30, 22, 581, DateTimeKind.Utc).AddTicks(1273));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 6, 30, 22, 581, DateTimeKind.Utc).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 6, 30, 22, 581, DateTimeKind.Utc).AddTicks(1277));

            migrationBuilder.CreateIndex(
                name: "IX_ComprehensiveTariffs_InsurancePlanId",
                table: "ComprehensiveTariffs",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprehensiveTariffs_ValueBandId",
                table: "ComprehensiveTariffs",
                column: "ValueBandId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprehensiveTariffs_VehicleCategoryId",
                table: "ComprehensiveTariffs",
                column: "VehicleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprehensiveTariffs_VehicleTypeId",
                table: "ComprehensiveTariffs",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprehensiveTariffs");

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
    }
}
