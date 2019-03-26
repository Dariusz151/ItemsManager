-- =============================================
-- Author:		Dariusz Koziol
-- Create date: 25/03/2019
-- Description:	Consume food items from given List<Ingredient>. 
-- Output: rows affected
-- =============================================
ALTER PROCEDURE [sp_U_ConsumeFoodItemsByList] 
	@ingredientsXML NVARCHAR(MAX),
	@userId uniqueidentifier 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @xml as XML;
	SET @xml = CAST(@ingredientsXML AS XML);

	MERGE INTO [dbo].[article] as [art]
	USING (
			SELECT 
				 T.c.value('Name[1]', 'NVARCHAR(100)') AS [Name]
				,T.c.value('Weight[1]', 'NVARCHAR(100)') AS [Weight]
			FROM   @xml.nodes('/ArrayOfIngredient/Ingredient') as T(c) 
          ) AS [src]
    ON 
		[src].[Name] = [art].[article_name]
	WHEN MATCHED THEN
	UPDATE SET
		[art].[weight] = [art].[weight] - [src].[Weight]
	OUTPUT
	   $action as [ActionType]
	   ,deleted.[weight]
	   ,deleted.[quantity]
	   ,deleted.[article_name]
	   ,deleted.[article_id];
END
GO
