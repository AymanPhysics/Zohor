alter table cases add Code nvarchar(100)
alter table deletedcases add Code nvarchar(100)

alter table cases add InOutDateTime DateTime
alter table deletedcases add InOutDateTime DateTime

go

alter TRIGGER Tr_Cases
   ON  Cases
   AFTER INSERT,update
AS 
BEGIN
	SET NOCOUNT ON;

	declare @Id bigint=(select top 1 Id from inserted)
	
	update cases
	set Code=dbo.NumToCode(@Id)
	where Id=@Id
END
GO

update cases
set Code=dbo.NumToCode(Id)

select id,name,code,dbo.NumToCode(Id) from cases order by id desc
