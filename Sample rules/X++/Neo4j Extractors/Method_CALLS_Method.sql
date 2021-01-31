
-- The run command below includes Method -[CALLS]-> Method information. This is harvested from the cross 
-- reference (xref) database using the query below. 
-- There are several ways this query can be used to generate a CSV file:
--
--  1) Use the SQLcmd tool:
--
--  sqlcmd -S . -d DYNAMICSXREFDB -E -s, -W -I script.sql -o output.csv 
--  
--  This generates a 2GB file but it contains some ugly markup (a set of --- lines to separate the
--  data from the headers - It is not clear how to get rid of that, except by doing:
--
--  sqlcmd ... | sed -e '2d'
--
--  2) Use the MSSQL tool. Execute the query from a query window.  In the grid view select 
--  Save Results As… and select Save As type = CSV. If headers are wanted (and they are for this usage) 
--  you would need to go into "Tools | Options | Query Results | SQL Server | Results to grid" and set the 
--  "Include column Headers when copying or saving the results" checkbox.
Use the "Export data…" facility on the database (under tasks) in MSSQL. It does work  (and you can provide a query) but it is cumbersome to set up.


select LOWER('/' + sourceModule.Module + source.[Path]) as ":START_ID", 
       LOWER('/' + targetModule.Module + target.[Path]) as ":END_ID", 
	   'CALLS' as ":TYPE",
       count(target.Path) as "Count:int"
from dbo.[References] as refs
join dbo.[Names] as source on refs.SourceId = source.Id
join dbo.Modules as sourceModule on source.ModuleId = sourceModule.Id
join dbo.[Names] as target on refs.TargetId = target.Id
join dbo.Modules as targetModule on target.ModuleId = targetModule.Id
-- Call. Even references to fields are marked as calls, prompting filtering below.
where refs.kind = 1 
and targetModule.Module not like 'kerneltypemodule' -- exclude .NET calls
and (target.Path like '/Classes/%/Methods/%'
or target.Path like '/Tables/%/Methods/%'
or target.Path like '/Queries/%/Methods/%'
or target.Path like '/Forms/%/Methods/%'
or target.Path like '/Views/%/Methods/%')
group by source.Path, sourceModule.Module, target.Path, targetModule.Module
