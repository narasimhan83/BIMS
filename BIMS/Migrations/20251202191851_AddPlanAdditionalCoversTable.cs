using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanAdditionalCoversTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalCovers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoverName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CoverNameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalCovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenefitTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleCategoryId = table.Column<int>(type: "int", nullable: false),
                    BenefitTypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BenefitTypeNameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitTypes_VehicleCategories_VehicleCategoryId",
                        column: x => x.VehicleCategoryId,
                        principalTable: "VehicleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanAdditionalCovers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePlanId = table.Column<int>(type: "int", nullable: false),
                    AdditionalCoverId = table.Column<int>(type: "int", nullable: false),
                    PremiumFixed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanAdditionalCovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanAdditionalCovers_AdditionalCovers_AdditionalCoverId",
                        column: x => x.AdditionalCoverId,
                        principalTable: "AdditionalCovers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanAdditionalCovers_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanBenefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePlanId = table.Column<int>(type: "int", nullable: false),
                    BenefitTypeId = table.Column<int>(type: "int", nullable: false),
                    IsCovered = table.Column<bool>(type: "bit", nullable: true),
                    LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExcessAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanBenefits_BenefitTypes_BenefitTypeId",
                        column: x => x.BenefitTypeId,
                        principalTable: "BenefitTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanBenefits_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 19, 18, 47, 355, DateTimeKind.Utc).AddTicks(3749));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 19, 18, 47, 355, DateTimeKind.Utc).AddTicks(3753));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 19, 18, 47, 355, DateTimeKind.Utc).AddTicks(3755));

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTypes_VehicleCategoryId",
                table: "BenefitTypes",
                column: "VehicleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanAdditionalCovers_AdditionalCoverId",
                table: "PlanAdditionalCovers",
                column: "AdditionalCoverId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanAdditionalCovers_InsurancePlanId",
                table: "PlanAdditionalCovers",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanBenefits_BenefitTypeId",
                table: "PlanBenefits",
                column: "BenefitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanBenefits_InsurancePlanId",
                table: "PlanBenefits",
                column: "InsurancePlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanAdditionalCovers");

            migrationBuilder.DropTable(
                name: "PlanBenefits");

            migrationBuilder.DropTable(
                name: "AdditionalCovers");

            migrationBuilder.DropTable(
                name: "BenefitTypes");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 8, 46, 2, 910, DateTimeKind.Utc).AddTicks(3769));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 8, 46, 2, 910, DateTimeKind.Utc).AddTicks(3773));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 8, 46, 2, 910, DateTimeKind.Utc).AddTicks(3775));
        }
    }
}
