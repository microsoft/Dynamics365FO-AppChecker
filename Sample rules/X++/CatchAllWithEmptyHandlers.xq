(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify catch all catch all handlers that are empty :)
(: @Category BestPractice :)
(: @Language Xpp :)

<CatchAllWithEmptyHandlers  Category='BestPractice' href='docs.microsoft.com/Socratex/CatchAllWithEmptyHandlers' Version='1.0'>
{
    for $artifact in (/Form | /Class | /Query | /Table)
    	for $handler in $artifact//Method//CatchAllValues/../(EmptyStatement | CompoundStatement[not(*)])
    		return <EmptyHandler Artifact='{$artifact/@Artifact}' 
    		        StartLine='{$handler/@StartLine}' StartCol='{$handler/@StartCol}'
    		        EndLine='{$handler/@EndLine}' EndCol='{$handler/@EndCol}' />
}
</CatchAllWithEmptyHandlers>