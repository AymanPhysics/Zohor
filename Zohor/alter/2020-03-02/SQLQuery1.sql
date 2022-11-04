alter table Conferences add IsFree int not null default 0
go
alter table Conferences add IsCalc int not null default 0, AttendanceHours float not null default 0
go

alter table Conferences add CertificateLeft2 float not null default 0, CertificateTop2 float not null default 0
go
