DECLARE @userId as NVARCHAR(100);
SET @userId = N'c497ed62-3944-4f09-85e0-d99459a841e6';
SET @userId = CAST(@userId as uniqueidentifier);

DECLARE @raw_xml as NVARCHAR(MAX);

SET @raw_xml = N'<?xml version="1.0" encoding="utf-16"?><ArrayOfIngredient xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><Ingredient><Name>pomarañcz</Name><Weight>3</Weight></Ingredient><Ingredient><Name>banan</Name><Weight>1</Weight></Ingredient></ArrayOfIngredient>';


exec sp_U_ConsumeFoodItemsByList @raw_xml, @userId