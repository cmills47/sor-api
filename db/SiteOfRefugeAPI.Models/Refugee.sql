CREATE TABLE [Refugee](
	[Id] [uniqueidentifier] NOT NULL,
	[Summary] [uniqueidentifier],
	[Contact] [uniqueidentifier],
 CONSTRAINT [PK_Refugee] PRIMARY KEY CLUSTERED 
(
	[Id]
),
 CONSTRAINT [FK_Refugee_Contact] FOREIGN KEY([Contact])
REFERENCES [Contact] ([Id]),
 CONSTRAINT [FK_Refugee_RefugeeSummary] FOREIGN KEY([Summary])
REFERENCES [RefugeeSummary] ([Id])
)
