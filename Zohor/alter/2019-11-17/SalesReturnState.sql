go
ct SalesReturnStates
go
delete SalesReturnStates
insert SalesReturnStates(Id,name)select 1,'”·Ì„'
insert SalesReturnStates(Id,name)select 2,' «·›'
go
alter table salesdetails add SalesReturnStateId int not null default 0
alter table deletedsalesdetails add SalesReturnStateId int not null default 0
alter table salesdetails add SalesReturnStateReason nvarchar(1000) not null default ''
alter table deletedsalesdetails add SalesReturnStateReason nvarchar(1000) not null default ''