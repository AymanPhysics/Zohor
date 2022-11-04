alter table salesdetails add ConsumptionQty int not null default 0
alter table deletedsalesdetails add ConsumptionQty int not null default 0

alter table salesdetails add ConsumptionRemainingQty int not null default 0
alter table deletedsalesdetails add ConsumptionRemainingQty int not null default 0
