--drop TABLE [dbo].[Chat]
go
CREATE TABLE [dbo].[Chat](
	[UserFromId] [bigint] NULL,
	[UserToId] [bigint] NULL,
	[Msg] [nvarchar](4000) NULL,
	[MyGetDate] [datetime] NOT NULL,
	[IsDelivered] [int] NOT NULL,
	[DeliveredDate] [datetime] NULL,
	[IsSeen] [int] NOT NULL,
	[SeenDate] [datetime] NULL,
	[Line] [bigint] IDENTITY(1,1) NOT NULL,
	[ToLevelId] [int] NOT NULL,
 CONSTRAINT [PK_Chat] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Chat] ADD  DEFAULT (getdate()) FOR [MyGetDate]
GO

ALTER TABLE [dbo].[Chat] ADD  DEFAULT ((0)) FOR [IsDelivered]
GO

ALTER TABLE [dbo].[Chat] ADD  DEFAULT ((0)) FOR [IsSeen]
GO

ALTER TABLE [dbo].[Chat] ADD  DEFAULT ((0)) FOR [ToLevelId]
GO


