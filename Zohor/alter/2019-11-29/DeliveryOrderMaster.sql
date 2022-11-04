alter table DeliveryOrderMaster add SalesFlagId int not null default 0
alter table deletedDeliveryOrderMaster add SalesFlagId int not null default 0

go


update DeliveryOrderMaster set SalesFlagId =13 where Flag=0
update deletedDeliveryOrderMaster set SalesFlagId =13 where Flag=0