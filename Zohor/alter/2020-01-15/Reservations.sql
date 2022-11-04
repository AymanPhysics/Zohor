alter table Reservations add Perc float not null default 0
alter table deletedReservations add Perc float not null default 0

go

ct ExitTypes
go

alter table cases add RelationId2 int not null default 0
alter table cases add ExitTypeId int not null default 0

alter table deletedcases add RelationId2 int not null default 0
alter table deletedcases add ExitTypeId int not null default 0
