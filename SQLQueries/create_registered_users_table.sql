CREATE TABLE registered_users (
    id_user int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	login nvarchar(50) NOT NULL UNIQUE,
    first_name varchar(255) NOT NULL,
	salt varbinary(50) NOT NULL,
	password varbinary(255) NOT NULL,
	email nvarchar(50),
	phone char(10),
	id_role int NOT NULL FOREIGN KEY REFERENCES [FridgeDB].[dbo].[user_roles](id_role)
);SET ANSI_NULLS ON
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