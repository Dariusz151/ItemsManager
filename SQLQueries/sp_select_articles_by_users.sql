USE dariusz151_smartfridge
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dariusz Koziol
-- Create date: 21/03/2019
-- Description:	Select users with their articles
-- =============================================
CREATE PROCEDURE sp_SA_ArticlesByUsers 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		usr.[login] as [User]
	  , art.article_name as [Article name]
	  , art.quantity as [Quantity]
	  , art_cat.category_name as [Category]
	FROM [dariusz151_smartfridge].[dbo].[registered_users] as usr
		INNER JOIN [dbo].[article] as art
			ON art.id_user = usr.id_user
		INNER JOIN [dbo].[articles_category] as art_cat
			ON art_cat.id_category = art.id_category
	ORDER BY usr.login
	
END
GO
