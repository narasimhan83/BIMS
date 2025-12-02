-- Manual SQL script to create PlanBenefits table
-- Run this against your BIMS database in SQL Server (SSMS or Azure Data Studio)
-- to support the Plan Benefits CRUD screens.

IF OBJECT_ID(N'[dbo].[PlanBenefits]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PlanBenefits](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [InsurancePlanId] INT NOT NULL,
        [BenefitTypeId] INT NOT NULL,
        [IsCovered] BIT NOT NULL CONSTRAINT [DF_PlanBenefits_IsCovered] DEFAULT(1),
        [LimitAmount] DECIMAL(18,2) NULL,
        [ExcessAmount] DECIMAL(18,2) NULL,
        [Remarks] NVARCHAR(500) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_PlanBenefits_IsActive] DEFAULT(1),
        [CreatedDate] DATETIME2 NOT NULL,
        [ModifiedDate] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(50) NULL,
        [ModifiedBy] NVARCHAR(50) NULL
    );

    ALTER TABLE [dbo].[PlanBenefits] WITH CHECK
    ADD CONSTRAINT [FK_PlanBenefits_InsurancePlans_InsurancePlanId]
        FOREIGN KEY([InsurancePlanId])
        REFERENCES [dbo].[InsurancePlans]([Id])
        ON DELETE NO ACTION;

    ALTER TABLE [dbo].[PlanBenefits] WITH CHECK
    ADD CONSTRAINT [FK_PlanBenefits_BenefitTypes_BenefitTypeId]
        FOREIGN KEY([BenefitTypeId])
        REFERENCES [dbo].[BenefitTypes]([Id])
        ON DELETE NO ACTION;
END