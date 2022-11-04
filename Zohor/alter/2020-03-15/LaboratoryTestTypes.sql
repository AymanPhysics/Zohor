alter table LaboratoryTestTypes  add AllTests int
go
update LaboratoryTestTypes  set AllTests =0 where AllTests  is null
go
