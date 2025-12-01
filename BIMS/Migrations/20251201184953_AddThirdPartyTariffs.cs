using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddThirdPartyTariffs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThirdPartyTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePlanId = table.Column<int>(type: "int", nullable: false),
                    EngineCapacityId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_ThirdPartyTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThirdPartyTariffs_EngineCapacities_EngineCapacityId",
                        column: x => x.EngineCapacityId,
                        principalTable: "EngineCapacities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThirdPartyTariffs_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThirdPartyTariffs_VehicleTypes_VehicleTypeId",
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
                value: new DateTime(2025, 12, 1, 18, 49, 51, 406, DateTimeKind.Utc).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 18, 49, 51, 406, DateTimeKind.Utc).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 18, 49, 51, 406, DateTimeKind.Utc).AddTicks(4933));

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyTariffs_EngineCapacityId",
                table: "ThirdPartyTariffs",
                column: "EngineCapacityId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyTariffs_InsurancePlanId",
                table: "ThirdPartyTariffs",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyTariffs_VehicleTypeId",
                table: "ThirdPartyTariffs",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirdPartyTariffs");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 8, 34, 3, 629, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 8, 34, 3, 629, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 30, 8, 34, 3, 629, DateTimeKind.Utc).AddTicks(2189));
        }
    }
}
