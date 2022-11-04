alter table ProductionItemCollectionMotionDetailsTo drop column Line 
go
alter table ProductionItemCollectionMotionDetailsTo add Line bigint identity(1,1)