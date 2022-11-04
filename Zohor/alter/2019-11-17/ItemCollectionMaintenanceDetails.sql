
drop table ItemCollectionMaintenanceDetails
go
CREATE TABLE [dbo].ItemCollectionMaintenanceDetails(
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO

drop table ItemCollectionMaintenanceDetailsFrom
go
CREATE TABLE [dbo].ItemCollectionMaintenanceDetailsFrom(
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO

drop table ItemCollectionMaintenanceDetailsTo
go
CREATE TABLE [dbo].ItemCollectionMaintenanceDetailsTo(
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO




































drop table DeletedItemCollectionMaintenanceDetails
go
CREATE TABLE [dbo].DeletedItemCollectionMaintenanceDetails(
		[DeletedDate] [datetime] NULL,
	[UserDelete] [int] NULL,
	[LastLine] [int] NULL,
	[State] [varchar](100) NULL,
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] ,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO
drop table DeletedItemCollectionMaintenanceDetailsFrom
go

CREATE TABLE [dbo].DeletedItemCollectionMaintenanceDetailsFrom(
		[DeletedDate] [datetime] NULL,
	[UserDelete] [int] NULL,
	[LastLine] [int] NULL,
	[State] [varchar](100) NULL,
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] ,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO
drop table DeletedItemCollectionMaintenanceDetailsTo
go
CREATE TABLE [dbo].DeletedItemCollectionMaintenanceDetailsTo(
		[DeletedDate] [datetime] NULL,
	[UserDelete] [int] NULL,
	[LastLine] [int] NULL,
	[State] [varchar](100) NULL,
	[StoreId] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NULL,
	[DayDate] [datetime] NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](1000) NULL,
	[Qty] [float] NULL,
	[TotalQty] [float] NULL,
	[Price] [float] NULL,
	[Value] [float] NULL,
	[Line] [int] ,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[UnitId] [int] NULL,
	[UnitQty] [float] NULL,
	[Barcode] [nvarchar](100) NULL,
	[SerialNo] [int] not NULL  DEFAULT 0,
	[AvgCost] [float] NULL,
	Notes nvarchar(1000)
) ON [PRIMARY]

GO
