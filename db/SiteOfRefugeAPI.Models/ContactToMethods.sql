CREATE TABLE [SiteOfRefugeAPI.Models].[ContactToMethods](
	[ContactId] [uniqueidentifier] NOT NULL,
	[ContactModeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [FK_ContactToMethods_Contact] FOREIGN KEY([ContactId])
REFERENCES [SiteOfRefugeAPI.Models].[Contact] ([Id]),
 CONSTRAINT [FK_ContactToMethods_ContactMode] FOREIGN KEY([ContactModeId])
REFERENCES [SiteOfRefugeAPI.Models].[ContactMode] ([Id])
)
