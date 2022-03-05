CREATE TABLE [SiteOfRefugeAPI.Models].[Restrictions](
	[Id] [int] NOT NULL,
	[description] [nvarchar](50) NOT NULL,
	[value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Restrictions] PRIMARY KEY CLUSTERED 
(
	[Id]
)
)
