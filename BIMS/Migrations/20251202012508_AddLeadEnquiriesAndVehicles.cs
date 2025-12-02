using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddLeadEnquiriesAndVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Leads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "LeadEnquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadId = table.Column<int>(type: "int", nullable: false),
                    ProductClassId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadEnquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadEnquiries_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadEnquiries_ProductClasses_ProductClassId",
                        column: x => x.ProductClassId,
                        principalTable: "ProductClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadEnquiries_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanExcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsurancePlanId = table.Column<int>(type: "int", nullable: false),
                    ExcessTypeId = table.Column<int>(type: "int", nullable: false),
                    ValueBandId = table.Column<int>(type: "int", nullable: false),
                    ExcessAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExcessUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanExcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanExcesses_ExcessTypes_ExcessTypeId",
                        column: x => x.ExcessTypeId,
                        principalTable: "ExcessTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanExcesses_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanExcesses_ValueBands_ValueBandId",
                        column: x => x.ValueBandId,
                        principalTable: "ValueBands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeadEnquiryVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadEnquiryId = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleMakeId = table.Column<int>(type: "int", nullable: true),
                    VehicleModelId = table.Column<int>(type: "int", nullable: true),
                    VehicleYearId = table.Column<int>(type: "int", nullable: true),
                    EngineCapacityId = table.Column<int>(type: "int", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    ExpectedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PolicyExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadEnquiryVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_EngineCapacities_EngineCapacityId",
                        column: x => x.EngineCapacityId,
                        principalTable: "EngineCapacities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_LeadEnquiries_LeadEnquiryId",
                        column: x => x.LeadEnquiryId,
                        principalTable: "LeadEnquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_VehicleMakes_VehicleMakeId",
                        column: x => x.VehicleMakeId,
                        principalTable: "VehicleMakes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_VehicleModels_VehicleModelId",
                        column: x => x.VehicleModelId,
                        principalTable: "VehicleModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadEnquiryVehicles_VehicleYears_VehicleYearId",
                        column: x => x.VehicleYearId,
                        principalTable: "VehicleYears",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 1, 25, 7, 45, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 1, 25, 7, 45, DateTimeKind.Utc).AddTicks(6106));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 2, 1, 25, 7, 45, DateTimeKind.Utc).AddTicks(6108));

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiries_LeadId",
                table: "LeadEnquiries",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiries_ProductClassId",
                table: "LeadEnquiries",
                column: "ProductClassId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiries_ProductTypeId",
                table: "LeadEnquiries",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_EngineCapacityId",
                table: "LeadEnquiryVehicles",
                column: "EngineCapacityId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_LeadEnquiryId",
                table: "LeadEnquiryVehicles",
                column: "LeadEnquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_VehicleMakeId",
                table: "LeadEnquiryVehicles",
                column: "VehicleMakeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_VehicleModelId",
                table: "LeadEnquiryVehicles",
                column: "VehicleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_VehicleTypeId",
                table: "LeadEnquiryVehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadEnquiryVehicles_VehicleYearId",
                table: "LeadEnquiryVehicles",
                column: "VehicleYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanExcesses_ExcessTypeId",
                table: "PlanExcesses",
                column: "ExcessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanExcesses_InsurancePlanId",
                table: "PlanExcesses",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanExcesses_ValueBandId",
                table: "PlanExcesses",
                column: "ValueBandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadEnquiryVehicles");

            migrationBuilder.DropTable(
                name: "PlanExcesses");

            migrationBuilder.DropTable(
                name: "LeadEnquiries");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Leads");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 46, 58, 762, DateTimeKind.Utc).AddTicks(9013));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 46, 58, 762, DateTimeKind.Utc).AddTicks(9016));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 46, 58, 762, DateTimeKind.Utc).AddTicks(9019));
        }
    }
}
