create TRIGGER Tr_Employees
   ON  Employees
   AFTER INSERT,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

    update Employees
    set Cashier=Nurse
    where Id in(select Id from inserted)
END
GO


alter table Customers add CashierId int not null default 0