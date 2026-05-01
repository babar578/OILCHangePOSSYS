-- ================================================
-- Website Leads Table for Public Website Lead Capture
-- ================================================
-- This script creates the WebsiteLeads table in ControlDB
-- Execute this script on your SQL Server instance

USE ControlDB;
GO

-- Create WebsiteLeads Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteLeads]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[WebsiteLeads] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [FullName] NVARCHAR(250) NOT NULL,
        [Company] NVARCHAR(250) NULL,
        [Email] NVARCHAR(320) NOT NULL,
        [Phone] NVARCHAR(50) NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [InterestedPlan] NVARCHAR(50) NULL,
        [Source] NVARCHAR(250) NULL,  -- Future usage: Facebook, Instagram, Web, etc.
        [Status] NVARCHAR(50) NULL DEFAULT 'New',  -- New, Contacted, Qualified, Converted, Lost
        [AssignedTo] INT NULL,  -- Future: Assign to admin user
        [Notes] NVARCHAR(MAX) NULL,
        [FollowUpDate] DATETIME2 NULL,
        [Country] NVARCHAR(100) NULL,
        [Language] NVARCHAR(50) NULL DEFAULT 'en',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [LastUpdated] DATETIME2 NULL,
        [IsActive] BIT NOT NULL DEFAULT 1
    );
    
    -- Create indexes for better query performance
    CREATE INDEX IX_WebsiteLeads_Email ON [dbo].[WebsiteLeads]([Email]);
    CREATE INDEX IX_WebsiteLeads_CreatedAt ON [dbo].[WebsiteLeads]([CreatedAt]);
    CREATE INDEX IX_WebsiteLeads_Status ON [dbo].[WebsiteLeads]([Status]);
    CREATE INDEX IX_WebsiteLeads_Source ON [dbo].[WebsiteLeads]([Source]);
    
    PRINT 'WebsiteLeads table created successfully.';
END
ELSE
BEGIN
    PRINT 'WebsiteLeads table already exists.';
END
GO

-- Display summary
PRINT '================================================';
PRINT 'WebsiteLeads Table Setup Complete!';
PRINT '================================================';
SELECT COUNT(*) AS TotalLeads FROM [dbo].[WebsiteLeads];
GO

