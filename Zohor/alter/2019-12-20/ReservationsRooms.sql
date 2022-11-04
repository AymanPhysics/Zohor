alter table ReservationsRooms  add IsClosed int not null default 0,CurrentShift int not null default 0
alter table ReservationsRoomsCanceled  add IsClosed int not null default 0,CurrentShift int not null default 0

alter table RoomsDataPayments  add IsClosed int not null default 0

alter table RoomsDataPayments  add InsertedDate datetime not null default getdate()

alter table ReservationsRooms  add InsertedDate datetime not null default getdate()

alter table RoomsDataPayments add MyMainLine bigint not null default 0
go

--update ReservationsRooms  set IsClosed =1




