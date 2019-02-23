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