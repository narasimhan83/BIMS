-- Manual SQL script to create BenefitTypes table
-- Run this against your BIMS database in SQL Server (SSMS or Azure Data Studio)
-- to fix: "SqlException: Invalid object name 'BenefitTypes'."

IF OBJECT_ID(N'[dbo].[BenefitTypes]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[BenefitTypes](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [VehicleCategoryId] INT NOT NULL,
        [BenefitTypeName] NVARCHAR(150) NOT NULL,
        [BenefitTypeNameAr] NVARCHAR(150) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_BenefitTypes_IsActive] DEFAULT(1),
        [CreatedDate] DATETIME2 NOT NULL,
        [ModifiedDate] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(50) NULL,
        [ModifiedBy] NVARCHAR(50) NULL
    );

    ALTER TABLE [dbo].[BenefitTypes] WITH CHECK
    ADD CONSTRAINT [FK_BenefitTypes_VehicleCategories_VehicleCategoryId]
        FOREIGN KEY([VehicleCategoryId])
        REFERENCES [dbo].[VehicleCategories]([Id])
        ON DELETE NO ACTION;
END