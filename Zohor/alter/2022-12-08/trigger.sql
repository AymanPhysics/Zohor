 

CREATE trigger TR_CasesComplaint on CasesComplaint
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_CasesComplaintDt on CasesComplaintDt
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_LabTests on LabTests
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_ProgressNote on ProgressNote
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_ReservationsClinics on ReservationsClinics
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_ReservationsOperations on ReservationsOperations
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_ReservationsRooms on ReservationsRooms
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
CREATE trigger TR_RoomsData on RoomsData
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO






CREATE trigger [dbo].[TR_services] on [dbo].services
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
 
CREATE trigger [dbo].[TR_Reservations] on [dbo].Reservations
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
 
CREATE trigger [dbo].[TR_OperationMotions] on [dbo].OperationMotions
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
 
CREATE trigger [dbo].[TR_ClinicsHistory] on [dbo].ClinicsHistory
FOR insert
as
update Cases set LastMotionDate=getdate()  where Id in(select CaseId from inserted)
GO
