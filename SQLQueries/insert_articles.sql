USE FridgeDB
GO

	INSERT INTO [dbo].[articles]
	(	
		[article_name]
		,[quantity]
		,[weight]
		,[id_user]
		,[id_category]
	)
	VALUES 
		( 'Kurczak', 1,  200, 1, 2),
		( 'Ry¿', 1,  100, 1, 2),
		( 'Cebula', 1,  50, 1, 3),
		( 'Quinoa', 1,  100, 1, 1),
		( 'Ziemniaki', 5,  50, 1, 3),
		( 'Frytki', 1,  700, 2, 1),
		( 'Sa³ata', 1,  100, 2, 2),
		( 'Kiwi', 3,  50, 2, 3),
		( 'Banan', 3,  100, 2, 1),
		( 'Gruszka', 5,  70, 1, 1),
		( 'Miêso wo³owe', 1,  500, 1, 1),
		( 'Bu³ka hamburgerowa', 2,  200, 2, 1),
		( 'Pomidor', 1,  100, 1, 2),
		( 'Nachos', 50,  10, 2, 1),
		( 'Rodzynki', 1,  100, 1, 3),
		( 'Ogórek', 3,  200, 1, 3),
		( 'Ser feta', 1,  250, 2, 3),
		( 'Kukurydza', 1,  250, 2, 2),
		( 'Makaron', 1,  400, 1, 1),
		( 'Miruna', 1,  400, 2, 2),
		( 'Tofu', 1,  100, 2, 2),
		( 'Szpinak', 10,  200, 2, 1),
		( 'Mleko', 2,  200, 2, 3)
