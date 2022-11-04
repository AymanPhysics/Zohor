truncate table Chat
go
create table Chat(
UserFromId bigint,
UserToId bigint,
Msg nvarchar(4000),
MyGetDate Datetime not null default getdate(),
IsDelivered int not null default 0,
DeliveredDate Datetime,
IsSeen int not null default 0,
SeenDate Datetime,
Line bigint identity(1,1)
)
go
alter proc SendChat
@UserFromId bigint,
@Msg nvarchar(4000),
@UserToIds Type_Id readonly

as

insert Chat(UserFromId,UserToId,Msg)
select @UserFromId,Id,@Msg
from @UserToIds

go

alter proc GetChatList
@UserToId bigint
as

select *
from(
	Select E.Id,E.Name,E.LevelId,L.Name LevelName,
	(
		select MAX(C0.Line) 
		from Chat C0 
		where 
			(
				(C0.UserFromId=E.Id and C0.UserToId=@UserToId)or
				(C0.UserToId=E.Id and C0.UserFromId=@UserToId)
			)
	)ChatLine,
	(
		select count(C0.Line) 
		from Chat C0 
		where IsSeen=0
		and C0.UserFromId=E.Id 
		and C0.UserToId=@UserToId
	)Count
	from Employees E
	left join Levels L on(E.LevelId=L.Id)
	where E.SystemUser=1
	and E.Stopped=0
	and E.Id<>@UserToId
)Tbl
left join Chat C on(Tbl.ChatLine=C.Line)
order by C.Line desc,Tbl.Name

go

alter proc GetChatOne
@UserFromId bigint,
@UserToId bigint
as

select *,E.Name,
(Case when UserFromId=@UserFromId then 1 else 0 end)IsMe
 from (
select top 100 *
from Chat C
where 
	(
		(C.UserFromId=@UserFromId and C.UserToId=@UserToId)or
		(C.UserToId=@UserFromId and C.UserFromId=@UserToId)
	)
order by Line desc
)T
left join Employees E on(T.UserFromId=E.Id)
order by Line 

go

create proc GetNewChat
@UserFromId bigint

as
select count(C.IsDelivered)
from Chat C
where C.UserToId=@UserFromId
and IsDelivered=0


go

alter proc SetNewChatIsDelivered
@UserFromId bigint,
@Line bigint
as
update Chat 
set IsDelivered=1,
DeliveredDate=GETDATE()
where UserToId=@UserFromId
and IsDelivered=0
and Line<=@Line
go


alter proc SetNewChatIsSeen
@UserFromId bigint,
@UserToId bigint,
@Line bigint
as
update Chat 
set IsSeen=1,
SeenDate=GETDATE()
where UserToId=@UserFromId
and UserFromId=@UserToId
and IsSeen=0
and Line<=@Line
go
