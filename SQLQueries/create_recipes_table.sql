USE [FridgeDB]
GO

CREATE TABLE [recipes] (
    [recipe_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[recipe_name] nvarchar(50) NOT NULL,
	[ingredients] nvarchar(1000) NOT NULL,
	[description] nvarchar(max) NOT NULL,
	[createdAt] DATETIME2(3) CONSTRAINT DF_recipes_Created DEFAULT (SYSDATETIME())
);

GO

CREATE NONCLUSTERED INDEX IX_recipes_recipes_name   
    ON [dbo].[recipes]([recipe_name]);   
GO
