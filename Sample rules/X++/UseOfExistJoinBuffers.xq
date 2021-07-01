(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identifies places where exists or notexists joins are used after they
   are selected in a search statement. This is sometimes indicative of a
   bug where the author believes that the buffer contains a record after
   the select, which is not the case.
   This query is likely to cause false positives. :)
(: @Category Informational :)
(: @Language Xpp :)

<UseOfExistJoinBuffers>
{
    for $a in /(Class | Table | Form)
    for $m in $a//Method
    for $ss in $m//SearchStatement
    for $buffer in $ss/Query//JoinSpecification[@Kind='ExistsJoin' or @Kind='NotExistsJoin']/Query
    for $useOfField in $ss/CompoundStatement//SimpleField[lower-case(@Name)=lower-case($buffer/@BufferName)] 
    order by $a/@Package
    let $i := $useOfField
    return <UseOfExistJoinBuffer Artifact='{$a/@Artifact}' Package='{$a/@Package}' Field='{$buffer/@BufferName}'
        StartLine='{$i/@StartLine}' StartCol='{$i/@StartCol}'
        EndLine='{$i/@EndLine}' EndCol='{$i/@EndCol}' />    
}
</UseOfExistJoinBuffers>
