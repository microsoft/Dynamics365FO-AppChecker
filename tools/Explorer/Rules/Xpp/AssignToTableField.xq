(: Find references to places where fields are updated :)
(: Note: This does not find references updated with byref parameters :)
<TableFieldAssignments>
{
for $toplevel in /* 
	for $t in $toplevel//(AssignEqualStatement | AssignIncrementStatement | AssignDecrementStatement)/QualifiedField[position()=1]
	   for $s in $t/SimpleQualifier/@Type
	   where /Table[@Name=$s]
		return <Assignment Field='{$t/@Name}' Type='{$s}' Artifact='{$toplevel/@Artifact}' 
		            StartLine='{$t/../@StartLine}' EndLine='{$t/../@EndLine}'
		            StarCol='{$t/../@StartCol}' EndCol='{$t/../@EndCol}'/> 
}
</TableFieldAssignments>