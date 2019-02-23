/****** Object:  Table [dbo].[articles_category]    Script Date: 05.01.2019 00:42:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[articles_category](
	[id_category] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [dbo].[articles_category] 
	(
		[category_name]
	)
VALUES
	( 'Warzywa'),
	( 'Miêsa'),
	( 'Owoce'),
	( 'Nabia³'),
	( 'Ryby'),
	( 'Orzechy'),
	( 'Napoje'),
	( 'S³odycze')

GO

/****** Object:  Table [dbo].[user_roles]    Script Date: 05.01.2019 00:47:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user_roles](
	[id_role] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [dbo].[user_roles] 
	(
		[role_name]
	)
VALUES
	( 'Administrator'),
	( 'U¿ytkownik'),
	( 'Goœæ'),
	( 'Goœæ2'),
	( 'Goœæ3')
	
GO

/****** Object:  Table [dbo].[registered_users]    Script Date: 05.01.2019 00:48:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[registered_users](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[first_name] [varchar](255) NOT NULL,
	[salt] [varbinary](50) NOT NULL,
	[password] [varbinary](255) NOT NULL,
	[email] [nvarchar](50) NULL,
	[phone] [char](10) NULL,
	[id_role] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[registered_users]  WITH CHECK ADD FOREIGN KEY([id_role])
REFERENCES [dbo].[user_roles] ([id_role])
GO



/****** Object:  Table [dbo].[article]    Script Date: 05.01.2019 00:49:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[article](
	[article_id] [int] IDENTITY(1,1) NOT NULL,
	[article_name] [nvarchar](50) NOT NULL,
	[quantity] [int] NULL,
	[weight] [int] NULL,
	[createdAt] [datetime] NOT NULL,
	[id_user] [int] NULL,
	[id_category] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[article_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[article]  WITH CHECK ADD FOREIGN KEY([id_category])
REFERENCES [dbo].[articles_category] ([id_category])
GO

ALTER TABLE [dbo].[article]  WITH CHECK ADD FOREIGN KEY([id_user])
REFERENCES [dbo].[registered_users] ([id_user])
GO


/****** Object:  Table [dbo].[recipes]    Script Date: 05.01.2019 00:49:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[recipes](
	[recipe_id] [int] IDENTITY(1,1) NOT NULL,
	[recipe_name] [nvarchar](50) NOT NULL,
	[ingredients] [nvarchar](1000) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[createdAt] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[recipe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[recipes] ADD  CONSTRAINT [DF_recipes_Created]  DEFAULT (sysdatetime()) FOR [createdAt]
GO

