CREATE TABLE [SiteOfRefugeAPI.Models].[HostSummaryToRestrictions](
	[HostSummaryId] [uniqueidentifier] NOT NULL,
	[RestrictionsId] [int] NOT NULL,
 CONSTRAINT [FK_HostSummaryToRestrictions_HostSummary] FOREIGN KEY([HostSummaryId])
REFERENCES [SiteOfRefugeAPI.Models].[HostSummary] ([Id]),
 CONSTRAINT [FK_HostSummaryToRestrictions_Restrictions] FOREIGN KEY([RestrictionsId])
REFERENCES [SiteOfRefugeAPI.Models].[Restrictions] ([Id])
)
