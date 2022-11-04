
CREATE TABLE SupliersPayments(
	MainLine [bigint] NOT NULL,
	StoreId int,
	InvoiceNo bigint,
	Value float,
	Line int Identity,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
)

