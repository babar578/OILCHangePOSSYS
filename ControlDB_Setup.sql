-- ================================================
-- Multi-Tenant Architecture: ControlDB Setup Script
-- ================================================
-- This script creates the ControlDB database for managing tenant information
-- Execute this script on your SQL Server instance

USE master;
GO

-- Create ControlDB Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ControlDB')
BEGIN
    CREATE DATABASE ControlDB;
    PRINT 'ControlDB database created successfully.';
END
ELSE
BEGIN
    PRINT 'ControlDB database already exists.';
END
GO

USE ControlDB;
GO

-- Create Tenants Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tenants]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Tenants] (
        [TenantId] INT PRIMARY KEY IDENTITY(1,1),
        [TenantName] NVARCHAR(100) NOT NULL UNIQUE,
        [TenantCode] NVARCHAR(50) NOT NULL UNIQUE,
        [DBServer] NVARCHAR(200) NOT NULL,
        [DBName] NVARCHAR(100) NOT NULL,
        [DBUser] NVARCHAR(100) NOT NULL,
        [DBPassword] NVARCHAR(200) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] DATETIME NULL
    );
    PRINT 'Tenants table created successfully.';
END
ELSE
BEGIN
    PRINT 'Tenants table already exists.';
END
GO

-- Create ControlUsers Table for tenant mapping
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControlUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControlUsers] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [UserName] NVARCHAR(100) NOT NULL UNIQUE,
        [TenantId] INT NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_ControlUsers_Tenants FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants]([TenantId])
    );
    PRINT 'ControlUsers table created successfully.';
END
ELSE
BEGIN
    PRINT 'ControlUsers table already exists.';
END
GO

-- Insert first tenant (existing database)
IF NOT EXISTS (SELECT * FROM [dbo].[Tenants] WHERE [TenantCode] = 'TENANT001')
BEGIN
    INSERT INTO [dbo].[Tenants] ([TenantName], [TenantCode], [DBServer], [DBName], [DBUser], [DBPassword], [IsActive])
    VALUES ('Shahzad Oil Store', 'TENANT001', 'localhost', 'itcorner_ShahzadOilStoreCentralPark', 'sa', 'Entrum786@', 1);
    PRINT 'First tenant (Shahzad Oil Store) inserted successfully.';
END
ELSE
BEGIN
    PRINT 'First tenant already exists.';
END
GO

-- Map existing users to tenant
-- Note: This assumes your existing database has a Users table with UserName and IsActive columns
DECLARE @TenantId INT = (SELECT TOP 1 TenantId FROM [dbo].[Tenants] WHERE TenantCode = 'TENANT001');

IF @TenantId IS NOT NULL
BEGIN
    -- Check if the source database exists
    IF EXISTS (SELECT name FROM sys.databases WHERE name = 'itcorner_ShahzadOilStoreCentralPark')
    BEGIN
        -- Insert users from existing database
        INSERT INTO [dbo].[ControlUsers] ([UserName], [TenantId], [IsActive])
        SELECT DISTINCT u.[UserName], @TenantId, u.[IsActive]
        FROM [itcorner_ShahzadOilStoreCentralPark].[dbo].[Users] u
        WHERE NOT EXISTS (
            SELECT 1 FROM [dbo].[ControlUsers] cu 
            WHERE cu.[UserName] = u.[UserName]
        );
        
        PRINT 'Existing users mapped to tenant successfully.';
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Source database [itcorner_ShahzadOilStoreCentralPark] not found. Please map users manually.';
    END
END
GO

-- Display summary
PRINT '================================================';
PRINT 'ControlDB Setup Complete!';
PRINT '================================================';
SELECT 'Tenants' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[Tenants]
UNION ALL
SELECT 'ControlUsers' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[ControlUsers];
GO

