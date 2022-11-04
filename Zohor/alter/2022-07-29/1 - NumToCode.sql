create FUNCTION NumToCode(
@n bigint
)
returns nvarchar(100)
as
begin

declare @i int=1
declare @str nvarchar(100)=''
while @n>0
begin
	--select @str+=cast(@n%36 as nvarchar(100))
	select @str=(case @n%36 
	when 0 then '0'
	when 1 then '1'
	when 2 then '2'
	when 3 then '3'
	when 4 then '4'
	when 5 then '5'
	when 6 then '6'
	when 7 then '7'
	when 8 then '8'
	when 9 then '9'
	when 10 then 'A'
	when 11 then 'B'
	when 12 then 'C'
	when 13 then 'D'
	when 14 then 'E'
	when 15 then 'F'
	when 16 then 'G'
	when 17 then 'H'
	when 18 then 'I'
	when 19 then 'J'
	when 20 then 'K'
	when 21 then 'L'
	when 22 then 'M'
	when 23 then 'N'
	when 24 then 'O'
	when 25 then 'P'
	when 26 then 'Q'
	when 27 then 'R'
	when 28 then 'S'
	when 29 then 'T'
	when 30 then 'U'
	when 31 then 'V'
	when 32 then 'W'
	when 33 then 'X'
	when 34 then 'Y'
	when 35 then 'Z'
	else '#' end)+@str
	select @n=(@n-@n%36)/36
	select @i+=1
end

return @str


end