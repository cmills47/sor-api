CREATE TABLE [SiteOfRefugeAPI.Models].[RefugeeSummaryToLanguages](
	[RefugeeSummaryId] [uniqueidentifier] NOT NULL,
	[SpokenLanguagesId] [int] NOT NULL,
 CONSTRAINT [FK_RefugeeSummaryToLanguages_RefugeeSummary] FOREIGN KEY([RefugeeSummaryId])
REFERENCES [SiteOfRefugeAPI.Models].[RefugeeSummary] ([Id]),
 CONSTRAINT [FK_RefugeeSummaryToLanguages_SpokenLanguages] FOREIGN KEY([SpokenLanguagesId])
REFERENCES [SiteOfRefugeAPI.Models].[SpokenLanguages] ([Id])
)
