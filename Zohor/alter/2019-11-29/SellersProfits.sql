

--drop TABLE [dbo].SellersProfits
go
CREATE TABLE [dbo].SellersProfits(
	[InvoiceNo] [int] NOT NULL,
	DayDate [datetime] NULL,
	FromDate [datetime] NULL,
	ToDate [datetime] NULL,
	CashierId [int] NULL,
	
	StoreId int,
	SalesFlagId int,
	SalesInvoiceNo int,
	
    StoreName nvarchar(100),
    FlagName nvarchar(100),
    SalesDayDate datetime,
    ToId int,
    ToName nvarchar(1000),

	
	
	
	[Line] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	TotalAfterDiscount [float] NULL,
	ReturnValue [float] NULL,
	Payed [float] NULL,
	Remaining [float] NULL,
	Perc [float] NULL,
	PercValue [float] NULL,
	[Notes] [nvarchar](1000) NULL,
	
) ON [PRIMARY]

GO


alter table Statics add SellersProfitsAccNo nvarchar(100) 
go
update Statics set SellersProfitsAccNo =''
go
