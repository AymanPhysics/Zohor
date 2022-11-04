 

CREATE TABLE [dbo].ItemCollectionMaintenanceMaster(
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[Notes] [varchar](4000) NULL,
	[DocNo] [varchar](4000) NULL,
	[ItemId] [bigint] NULL,
	[MainQty] [float] NULL,
	[MotionTypeId] [int] NULL,
	[Temp] [int] NULL,
	[SystemUser] [int] NOT NULL,
	[DayDate2] [datetime] NULL,
	[Done] [int] NULL,
 CONSTRAINT [PK_ItemCollectionMaintenanceMaster] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC,
	[InvoiceNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ItemCollectionMaintenanceMaster] ADD  DEFAULT ((0)) FOR [SystemUser]
GO

 
 

CREATE TABLE [dbo].DeletedItemCollectionMaintenanceMaster(
	[DeletedDate] [datetime] NULL,
	[UserDelete] [int] NULL,
	[LastLine] [int] NULL,
	[State] [varchar](100) NULL,
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[Notes] [varchar](4000) NULL,
	[DocNo] [varchar](4000) NULL,
	[ItemId] [bigint] NULL,
	[MainQty] [float] NULL,
	[MotionTypeId] [int] NULL,
	[Temp] [int] NULL,
	[SystemUser] [int] NOT NULL,
	[DayDate2] [datetime] NULL,
	[Done] [int] NULL,
 ) ON [PRIMARY]

GO 

alter table ItemCollectionMaintenanceMaster add ToId bigint, Price float
alter table DeletedItemCollectionMaintenanceMaster add ToId bigint, Price float
go
