(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract entity method information in CSV format :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes'}

let $r := <MethodsOnMaps>
{
    for $a in /Map
    for $m in $a/Method
    let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private"
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected'
                  else if (lower-case($m/@IsPublic) = 'true') then "public"
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "public"
    return <Record>
        <Artifact name='Artifact:ID'>{lower-case("/" || $a/@Package || "/maps/" || $a/@Name || "/methods/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Artifact name='Artifact'>{lower-case($a/@Artifact)}</Artifact>
        <Map name='Map'>{lower-case($a/@Name)}</Map>
        <Method name='Name'>{lower-case($m/@Name)}</Method>
        <Kind name="Kind">Method</Kind>
        <IsAbstract name='IsAbstract:Boolean'>{lower-case($m/@IsAbstract)}</IsAbstract>
        <IsFinal name='IsFinal:Boolean'>{lower-case($m/@IsFinal)}</IsFinal>
        <IsStatic name='IsStatic:Boolean'>{lower-case($m/@IsStatic)}</IsStatic>
        <Visibility name='Visibility'>{string($visibility)}</Visibility>
        <StartLine name='StartLine:int'>{xs:integer($m/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($m/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($m/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($m/@EndCol)}</EndCol>
        <CMP name='CMP:int'>{local:MethodComplexity($m)}</CMP>
        <LOC name='LOC:int'>{$m/@EndLine - $m/@StartLine + 1}</LOC>
        <NOS name='NOS:int'>{count(for $stmt in $m//* where ends-with(name($stmt), 'Statement') return $stmt)}</NOS>
        <Label name=':LABEL'>Method</Label>>
     </Record>
}
</MethodsOnMaps>

return csv:serialize($r, $options)
