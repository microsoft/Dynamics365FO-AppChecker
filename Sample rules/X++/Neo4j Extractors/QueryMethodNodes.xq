(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information on methods on queries in CSV format :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnQueries>
{
    for $a in /Query/Class
    for $m in $a/Method
    let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private"
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected'
                  else if (lower-case($m/@IsPublic) = 'true') then "public"
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "public"
    let $name := tokenize($a/@Name, "\$")[2]
    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/../@Package || "/queries/" || $name || "/methods/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case(data($a/../@Package))}</Package>
        <Query name='Query'>{lower-case($a/@Name)}</Query>
        <Method name='Name'>{lower-case($m/@Name)}</Method>
        <Kind name="Kind">Method</Kind>
        <IsAbstract name='IsAbstract:Boolean'>{lower-case($m/@IsAbstract)}</IsAbstract>
        <IsStatic name='IsStatic:Boolean'>{lower-case($m/@IsStatic)}</IsStatic>
        <IsFinal name='IsFinal:Boolean'> {lower-case($m/@IsFinal)}</IsFinal>
        <Visibility name='Visibility'>{string($visibility)}</Visibility>
        <StartLine name='StartLine:int'>{xs:integer($m/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($m/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($m/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($m/@EndCol)}</EndCol>
        <CMP name='CMP:int'>{local:MethodComplexity($m)}</CMP>
        <LOC name='LOC:int'>{$m/@EndLine - $m/@StartLine + 1}</LOC>
        <NOS name='NOS:int'>{count(for $stmt in $m//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
        <Label name=':LABEL'>Method</Label>

     </Record>
}
</MethodsOnQueries>

return csv:serialize($r, $options)
