(: Identify catch all catch all handlers that are empty :)
<EmptyHandlers>
{
    for $artifact in (/Form | /Class | /Query | /Table)
    	for $handler in $artifact//Method//CatchAllValues/../(EmptyStatement | CompoundStatement[not(*)])
    		return <EmptyHandler Artifact='{$artifact/@Artifact}' 
    		        StartLine='{$handler/@StartLine}' StartCol='{$handler/@StartCol}'
    		        EndLine='{$handler/@EndLine}' EndCol='{$handler/@EndCol}' />
}
</EmptyHandlers>