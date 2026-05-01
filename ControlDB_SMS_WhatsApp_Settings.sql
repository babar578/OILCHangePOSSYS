-- =============================================
-- SMS and WhatsApp Settings Tables
-- =============================================
-- This script creates tables for SMS and WhatsApp configuration
-- These tables should be created in the tenant database (not ControlDB)
-- Each tenant will have their own SMS/WhatsApp settings

USE [itcorner_ShahzadOilStoreCentralPark]  -- Replace with your tenant database name
GO

-- =============================================
-- SMS Settings Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SMSSettings](
        [Id] [INT] IDENTITY(1,1) NOT NULL,
        [IsEnabled] [BIT] NOT NULL DEFAULT 0,
        [ProviderName] [NVARCHAR](100) NULL,
        [ApiUrl] [NVARCHAR](500) NULL,
        [ApiKey] [NVARCHAR](500) NULL,
        [ApiToken] [NVARCHAR](500) NULL,
        [SenderNumber] [NVARCHAR](50) NULL,
        [SenderId] [NVARCHAR](100) NULL,
        [Username] [NVARCHAR](100) NULL,
        [Password] [NVARCHAR](500) NULL,
        [Mask] [NVARCHAR](100) NULL,
        [AdditionalConfig] [NVARCHAR](MAX) NULL,
        [CreatedDate] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
        [ModifiedDate] [DATETIME2] NULL,
        [CreatedBy] [INT] NULL,
        [ModifiedBy] [INT] NULL,
        CONSTRAINT [PK_SMSSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    -- Insert default record (disabled)
    INSERT INTO [dbo].[SMSSettings] ([IsEnabled], [ProviderName], [ApiUrl], [SenderNumber], [Mask])
    VALUES (0, 'Telenor Corporate SMS', 'https://telenorcsms.com.pk:27677/corporate_sms2/api', NULL, 'Shahzad Oil')
    
    PRINT 'SMSSettings table created successfully'
END
ELSE
BEGIN
    PRINT 'SMSSettings table already exists'
END
GO

-- =============================================
-- WhatsApp Settings Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WhatsAppSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[WhatsAppSettings](
        [Id] [INT] IDENTITY(1,1) NOT NULL,
        [IsEnabled] [BIT] NOT NULL DEFAULT 0,
        [ProviderName] [NVARCHAR](100) NULL,
        [ApiUrl] [NVARCHAR](500) NULL,
        [ApiKey] [NVARCHAR](500) NULL,
        [ApiToken] [NVARCHAR](500) NULL,
        [PhoneNumber] [NVARCHAR](50) NULL,
        [SenderNumber] [NVARCHAR](50) NULL,
        [InstanceId] [NVARCHAR](100) NULL,
        [AccessToken] [NVARCHAR](500) NULL,
        [AdditionalConfig] [NVARCHAR](MAX) NULL,
        [CreatedDate] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
        [ModifiedDate] [DATETIME2] NULL,
        [CreatedBy] [INT] NULL,
        [ModifiedBy] [INT] NULL,
        CONSTRAINT [PK_WhatsAppSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    -- Insert default record (disabled)
    INSERT INTO [dbo].[WhatsAppSettings] ([IsEnabled], [ProviderName])
    VALUES (0, 'WhatsApp Business API')
    
    PRINT 'WhatsAppSettings table created successfully'
END
ELSE
BEGIN
    PRINT 'WhatsAppSettings table already exists'
END
GO

-- =============================================
-- Create Indexes for better performance
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SMSSettings_IsEnabled' AND object_id = OBJECT_ID('SMSSettings'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SMSSettings_IsEnabled] ON [dbo].[SMSSettings] ([IsEnabled])
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppSettings_IsEnabled' AND object_id = OBJECT_ID('WhatsAppSettings'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_WhatsAppSettings_IsEnabled] ON [dbo].[WhatsAppSettings] ([IsEnabled])
END

PRINT 'SMS and WhatsApp Settings tables created successfully!'
GO





