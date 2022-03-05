CREATE TABLE [SiteOfRefugeAPI.Models].[Refugee](
	[Id] [uniqueidentifier] NOT NULL,
	[Summary] [uniqueidentifier],
	[Contact] [uniqueidentifier],
 CONSTRAINT [PK_Refugee] PRIMARY KEY CLUSTERED 
(
	[Id]
),
 CONSTRAINT [FK_Refugee_Contact] FOREIGN KEY([Contact])
REFERENCES [SiteOfRefugeAPI.Models].[Contact] ([Id]),
 CONSTRAINT [FK_Refugee_RefugeeSummary] FOREIGN KEY([Summary])
REFERENCES [SiteOfRefugeAPI.Models].[RefugeeSummary] ([Id])
)
