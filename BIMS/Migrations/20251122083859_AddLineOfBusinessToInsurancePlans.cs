using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddLineOfBusinessToInsurancePlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Ensure LineOfBusinessId column exists and is NULLable (no default 0)
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.InsurancePlans', 'LineOfBusinessId') IS NULL
BEGIN
    ALTER TABLE [InsurancePlans]
    ADD [LineOfBusinessId] INT NULL;
END
ELSE
BEGIN
    DECLARE @dfName NVARCHAR(128);

    SELECT @dfName = df.name
    FROM sys.default_constraints df
    INNER JOIN sys.columns c
        ON df.parent_object_id = c.object_id
       AND df.parent_column_id = c.column_id
    WHERE df.parent_object_id = OBJECT_ID('dbo.InsurancePlans')
      AND c.name = 'LineOfBusinessId';

    IF @dfName IS NOT NULL
    BEGIN
        DECLARE @sql NVARCHAR(MAX);
        SET @sql = N'ALTER TABLE dbo.InsurancePlans DROP CONSTRAINT [' + @dfName + N']';
        EXEC(@sql);
    END

    ALTER TABLE [InsurancePlans] ALTER COLUMN [LineOfBusinessId] INT NULL;
END
");

            // 2. Clear any invalid 0 values to NULL
            migrationBuilder.Sql(@"
UPDATE [InsurancePlans]
SET LineOfBusinessId = NULL
WHERE ISNULL(LineOfBusinessId, 0) = 0;
");

            // 3. For existing plans, set LineOfBusinessId to a valid LineOfBusiness for the same InsuranceClient when possible
            migrationBuilder.Sql(@"
UPDATE ip
SET LineOfBusinessId = lob.Id
FROM InsurancePlans ip
CROSS APPLY (
    SELECT TOP (1) Id
    FROM LinesOfBusiness lob
    WHERE lob.InsuranceClientId = ip.InsuranceClientId
    ORDER BY Id
) lob
WHERE LineOfBusinessId IS NULL;
");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 22, 8, 38, 58, 492, DateTimeKind.Utc).AddTicks(8615));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 22, 8, 38, 58, 492, DateTimeKind.Utc).AddTicks(8618));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 22, 8, 38, 58, 492, DateTimeKind.Utc).AddTicks(8620));

            // 4. Create index only if it doesn't already exist
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_InsurancePlans_LineOfBusinessId'
      AND object_id = OBJECT_ID('dbo.InsurancePlans')
)
BEGIN
    CREATE INDEX [IX_InsurancePlans_LineOfBusinessId]
    ON [InsurancePlans] ([LineOfBusinessId]);
END
");

            // 5. Add FK only if it doesn't already exist
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_InsurancePlans_LinesOfBusiness_LineOfBusinessId'
      AND parent_object_id = OBJECT_ID('dbo.InsurancePlans')
)
BEGIN
    ALTER TABLE [InsurancePlans] WITH CHECK
    ADD CONSTRAINT [FK_InsurancePlans_LinesOfBusiness_LineOfBusinessId]
        FOREIGN KEY ([LineOfBusinessId])
        REFERENCES [LinesOfBusiness] ([Id])
        ON DELETE NO ACTION;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsurancePlans_LinesOfBusiness_LineOfBusinessId",
                table: "InsurancePlans");

            migrationBuilder.DropIndex(
                name: "IX_InsurancePlans_LineOfBusinessId",
                table: "InsurancePlans");

            migrationBuilder.DropColumn(
                name: "LineOfBusinessId",
                table: "InsurancePlans");

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 7, 37, 9, 205, DateTimeKind.Utc).AddTicks(5169));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 7, 37, 9, 205, DateTimeKind.Utc).AddTicks(5172));

            migrationBuilder.UpdateData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 7, 37, 9, 205, DateTimeKind.Utc).AddTicks(5173));
        }
    }
}
