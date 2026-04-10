-- ====================================================================
-- Script to create the database and necessary tables
-- ====================================================================

-- 1. Create the Database if it doesn't already exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StudentMgmtDb')
BEGIN
    CREATE DATABASE [StudentMgmtDb];
END
GO

USE [StudentMgmtDb];
GO

-- 2. Create the Users Table for Authentication
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id]           INT            IDENTITY (1, 1) NOT NULL,
        [Username]     NVARCHAR (50)  NOT NULL,
        [PasswordHash] NVARCHAR (MAX) NOT NULL,
        [Role]         NVARCHAR (MAX) DEFAULT N'Admin' NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- 3. Create the Students Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Students] (
        [Id]          INT            IDENTITY (1, 1) NOT NULL,
        [Name]        NVARCHAR (100) NOT NULL,
        [Email]       NVARCHAR (100) NOT NULL,
        [Age]         INT            NOT NULL,
        [Course]      NVARCHAR (MAX) NOT NULL,
        [CreatedDate] DATETIME2 (7)  DEFAULT GETUTCDATE() NOT NULL,
        CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- ====================================================================
-- Seed Default Data
-- ====================================================================

-- Insert default admin user if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [Role])
    VALUES ('admin', 'admin123', 'Admin');
END
GO
