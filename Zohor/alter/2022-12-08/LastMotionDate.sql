alter table Cases Add LastMotionDate datetime 
go
update Cases set LastMotionDate ='1900-1-1' where LastMotionDate  is null
go
