update salesmaster set temp2=1


update T
set Unit=(select TT.Name from ItemUnits TT where TT.Id=T.ItemUnitId)
from Items T
