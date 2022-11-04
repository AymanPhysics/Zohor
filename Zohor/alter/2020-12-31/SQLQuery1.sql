

alter table EmpCloseShift add SaveId int not null default 0,SaveGetDate datetime,SaveUsername int
alter table EmpCloseShift add SaveDate datetime
go
--update EmpCloseShift set SaveId =-1
go

alter table Statics add InSaveAccNo bigint,InSaveSubAccNo int

--update Statics set InSaveAccNo ='5120',InSaveSubAccNo =0