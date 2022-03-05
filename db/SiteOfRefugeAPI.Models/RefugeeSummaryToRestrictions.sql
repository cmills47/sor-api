CREATE TABLE [SiteOfRefugeAPI.Models].[RefugeeSummaryToRestrictions](
	[RefugeeSummaryId] [uniqueidentifier] NOT NULL,
	[RestrictionsId] [int] NOT NULL,
 CONSTRAINT [FK_RefugeeSummaryToRestrictions_RefugeeSummary] FOREIGN KEY([RefugeeSummaryId])
REFERENCES [SiteOfRefugeAPI.Models].[RefugeeSummary] ([Id]),
 CONSTRAINT [FK_RefugeeSummaryToRestrictions_Restrictions] FOREIGN KEY([RestrictionsId])
REFERENCES [SiteOfRefugeAPI.Models].[Restrictions] ([Id])
)
