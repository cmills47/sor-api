CREATE TABLE [SiteOfRefugeAPI.Models].[HostSummaryToLanguages](
	[HostSummaryId] [uniqueidentifier] NOT NULL,
	[SpokenLanguagesId] [int] NOT NULL,
 CONSTRAINT [FK_HostSummaryToLanguages_HostSummary] FOREIGN KEY([HostSummaryId])
REFERENCES [SiteOfRefugeAPI.Models].[HostSummary] ([Id]),
 CONSTRAINT [FK_HostSummaryToLanguages_SpokenLanguages] FOREIGN KEY([SpokenLanguagesId])
REFERENCES [SiteOfRefugeAPI.Models].[SpokenLanguages] ([Id])
)
