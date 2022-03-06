CREATE TABLE [SiteOfRefugeAPI.Models].[Invite](
	[Id] [uniqueidentifier] NOT NULL,
	[RefugeeId] [uniqueidentifier] NOT NULL,
	[HostId] [uniqueidentifier] NOT NULL,
	[DateRequested] [datetimeoffset](4) NULL,
	[DateAccepted] [datetimeoffset](4) NULL,
 CONSTRAINT [PK_Invite] PRIMARY KEY CLUSTERED 
(
	[Id]
)
)
