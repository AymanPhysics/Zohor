--update SalesFlags set Name='�������� ����' where Id=17
--update SalesFlags set Name='������� �������� ����' where Id=18

go
alter table salesmaster add CurrentShift int not null default 0
alter table deletedsalesmaster add CurrentShift int not null default 0