-- FU News Management System - Database Initialization Script
-- This script creates the database schema and seeds initial data

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FUNewsManagement')
BEGIN
    CREATE DATABASE FUNewsManagement;
END
GO

USE FUNewsManagement;
GO

-- =============================================
-- Create Tables
-- =============================================

-- SystemAccount Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemAccount')
BEGIN
    CREATE TABLE SystemAccount (
        AccountID SMALLINT IDENTITY(1,1) PRIMARY KEY,
        AccountName NVARCHAR(100) NULL,
        AccountEmail NVARCHAR(100) NOT NULL UNIQUE,
        AccountRole INT NOT NULL DEFAULT 1,
        AccountPassword NVARCHAR(500) NULL
    );
END
GO

-- Category Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Category')
BEGIN
    CREATE TABLE Category (
        CategoryID SMALLINT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100) NOT NULL,
        CategoryDesciption NVARCHAR(250) NULL,
        ParentCategoryID SMALLINT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentCategoryID) REFERENCES Category(CategoryID)
    );
END
GO

-- NewsArticle Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NewsArticle')
BEGIN
    CREATE TABLE NewsArticle (
        NewsArticleID NVARCHAR(20) PRIMARY KEY,
        NewsTitle NVARCHAR(200) NOT NULL,
        Headline NVARCHAR(500) NOT NULL,
        CreatedDate DATETIME NULL DEFAULT GETDATE(),
        NewsContent NVARCHAR(MAX) NOT NULL,
        NewsSource NVARCHAR(400) NULL,
        CategoryID SMALLINT NOT NULL,
        NewsStatus BIT NULL DEFAULT 1,
        CreatedByID SMALLINT NOT NULL,
        UpdatedByID SMALLINT NULL,
        ModifiedDate DATETIME NULL,
        CONSTRAINT FK_NewsArticle_Category FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
        CONSTRAINT FK_NewsArticle_CreatedBy FOREIGN KEY (CreatedByID) REFERENCES SystemAccount(AccountID)
    );
END
GO

-- Tag Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tag')
BEGIN
    CREATE TABLE Tag (
        TagID INT IDENTITY(1,1) PRIMARY KEY,
        TagName NVARCHAR(50) NOT NULL UNIQUE,
        Note NVARCHAR(400) NULL
    );
END
GO

-- NewsTag Junction Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NewsTag')
BEGIN
    CREATE TABLE NewsTag (
        NewsArticleID NVARCHAR(20) NOT NULL,
        TagID INT NOT NULL,
        PRIMARY KEY (NewsArticleID, TagID),
        CONSTRAINT FK_NewsTag_Article FOREIGN KEY (NewsArticleID) REFERENCES NewsArticle(NewsArticleID) ON DELETE CASCADE,
        CONSTRAINT FK_NewsTag_Tag FOREIGN KEY (TagID) REFERENCES Tag(TagID) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Seed Initial Data
-- =============================================

-- Seed Staff Account (Password: Staff@123 - BCrypt hashed)
IF NOT EXISTS (SELECT * FROM SystemAccount WHERE AccountEmail = 'staff@funews.com')
BEGIN
    INSERT INTO SystemAccount (AccountName, AccountEmail, AccountRole, AccountPassword)
    VALUES (N'Staff User', 'staff@funews.com', 1, '$2a$11$ZG0.2KAI0nLXAQy8VqjYi.Y0h3qXqQs3wI0X0HT0XvGdmz8YpX8QK');
END
GO

-- Seed Lecturer Account (Password: Lecturer@123 - BCrypt hashed)
IF NOT EXISTS (SELECT * FROM SystemAccount WHERE AccountEmail = 'lecturer@funews.com')
BEGIN
    INSERT INTO SystemAccount (AccountName, AccountEmail, AccountRole, AccountPassword)
    VALUES (N'Lecturer User', 'lecturer@funews.com', 2, '$2a$11$ZG0.2KAI0nLXAQy8VqjYi.Y0h3qXqQs3wI0X0HT0XvGdmz8YpX8QK');
END
GO

-- Seed Categories
IF NOT EXISTS (SELECT * FROM Category WHERE CategoryName = N'Technology')
BEGIN
    INSERT INTO Category (CategoryName, CategoryDesciption, IsActive) VALUES
    (N'Technology', N'Technology and Innovation News', 1),
    (N'Education', N'Education and Academic News', 1),
    (N'Sports', N'Sports Activities and Events', 1),
    (N'Culture', N'Cultural Events and Activities', 1),
    (N'Science', N'Scientific Research and Discoveries', 1);
END
GO

-- Seed Tags
IF NOT EXISTS (SELECT * FROM Tag WHERE TagName = N'FPT University')
BEGIN
    INSERT INTO Tag (TagName, Note) VALUES
    (N'FPT University', N'News related to FPT University'),
    (N'Student Life', N'Student activities and lifestyle'),
    (N'Research', N'Academic research and publications'),
    (N'Events', N'University events and activities'),
    (N'Announcements', N'Official announcements'),
    (N'Technology', N'Tech-related topics'),
    (N'Innovation', N'Innovation and startups');
END
GO

-- Seed Sample News Articles
DECLARE @StaffID SMALLINT;
SELECT @StaffID = AccountID FROM SystemAccount WHERE AccountEmail = 'staff@funews.com';

IF @StaffID IS NOT NULL AND NOT EXISTS (SELECT * FROM NewsArticle WHERE NewsArticleID = 'NEWS0001')
BEGIN
    INSERT INTO NewsArticle (NewsArticleID, NewsTitle, Headline, NewsContent, NewsSource, CategoryID, NewsStatus, CreatedByID)
    VALUES 
    ('NEWS0001', N'FPT University Launches New AI Lab', 
     N'A state-of-the-art Artificial Intelligence laboratory opens at FPT University', 
     N'FPT University has officially launched its new Artificial Intelligence laboratory equipped with the latest hardware and software for AI research and development. The lab will serve as a hub for students and researchers to explore machine learning, deep learning, and other AI technologies.',
     N'FPT University News', 1, 1, @StaffID),
    
    ('NEWS0002', N'Annual Sports Festival 2024', 
     N'FPT University hosts the biggest sports festival of the year', 
     N'The annual sports festival brings together students from all campuses to compete in various sports including football, basketball, volleyball, and badminton. This year''s festival features over 500 participants and promises exciting competitions.',
     N'FPT Sports', 3, 1, @StaffID),
    
    ('NEWS0003', N'New Scholarship Program Announced', 
     N'FPT University expands its scholarship offerings for academic year 2024-2025', 
     N'FPT University has announced a new scholarship program that will benefit outstanding students. The program includes full tuition scholarships, partial scholarships, and research grants for qualified applicants.',
     N'FPT Education', 2, 1, @StaffID);
END
GO

-- Link News with Tags
IF EXISTS (SELECT * FROM NewsArticle WHERE NewsArticleID = 'NEWS0001') 
   AND NOT EXISTS (SELECT * FROM NewsTag WHERE NewsArticleID = 'NEWS0001')
BEGIN
    INSERT INTO NewsTag (NewsArticleID, TagID)
    SELECT 'NEWS0001', TagID FROM Tag WHERE TagName IN (N'FPT University', N'Technology', N'Innovation');
    
    INSERT INTO NewsTag (NewsArticleID, TagID)
    SELECT 'NEWS0002', TagID FROM Tag WHERE TagName IN (N'FPT University', N'Events', N'Student Life');
    
    INSERT INTO NewsTag (NewsArticleID, TagID)
    SELECT 'NEWS0003', TagID FROM Tag WHERE TagName IN (N'FPT University', N'Announcements', N'Student Life');
END
GO

PRINT 'Database initialization completed successfully!';
GO
