CREATE TABLE articles (
    id_article int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	article_name nvarchar(50) NOT NULL,
	quantity int,
	weight int,
	id_user int FOREIGN KEY REFERENCES [FridgeDB].[dbo].[registered_users](id_user),
	id_category int FOREIGN KEY REFERENCES [FridgeDB].[dbo].[articles_category](id_category)
);

GO

CREATE NONCLUSTERED INDEX IX_articles_article_name   
    ON [dbo].[articles](article_name);   
GO