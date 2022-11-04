
alter table SalesMaster add PrintUser int not null default 0,PrintDate datetime
alter table deletedSalesMaster add PrintUser int,PrintDate datetime