CREATE TABLE [article] (
    [article_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[article_name] nvarchar(50) NOT NULL,
	[quantity] int NULL,
	[weight] int NULL,
	[createdAt] datetime not null,
	[id_user] int FOREIGN KEY REFERENCES [FridgeDB].[dbo].[registered_users](id_user),
	[id_category] int FOREIGN KEY REFERENCES [FridgeDB].[dbo].[articles_category](id_category)
);

GO

CREATE NONCLUSTERED INDEX IX_articles_article_name   
    ON [dbo].[articles](article_name);   
GO