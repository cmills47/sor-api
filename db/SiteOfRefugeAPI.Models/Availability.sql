CREATE TABLE [Availability](
	[Id] [uniqueidentifier] NOT NULL,
	[DateAvailable] [datetimeoffset](4) NULL,
	[Active] [bit] NULL,
	[LengthOfStay] [int],
 CONSTRAINT [PK_Availability] PRIMARY KEY CLUSTERED 
(
	[Id]
),
 CONSTRAINT [FK_Availability_AvailabilityLengthOfStay] FOREIGN KEY([LengthOfStay])
REFERENCES [AvailabilityLengthOfStay] ([Id])
)
