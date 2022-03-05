CREATE TABLE [SiteOfRefugeAPI.Models].[Host](
	[Id] [uniqueidentifier] NOT NULL,
	[Summary] [uniqueidentifier],
	[Contact] [uniqueidentifier],
 CONSTRAINT [PK_Host] PRIMARY KEY CLUSTERED 
(
	[Id]
),
 CONSTRAINT [FK_Host_Contact] FOREIGN KEY([Contact])
REFERENCES [SiteOfRefugeAPI.Models].[Contact] ([Id]),
 CONSTRAINT [FK_Host_HostSummary] FOREIGN KEY([Summary])
REFERENCES [SiteOfRefugeAPI.Models].[HostSummary] ([Id])
)
