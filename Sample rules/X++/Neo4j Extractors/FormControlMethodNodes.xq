(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information on methods on forms datasources in CSV format :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnForms>
{
    for $a in /Form
    for $datasource in $a/FormControl
    for $m in $datasource/Method
    let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private"
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected'
                  else if (lower-case($m/@IsPublic) = 'true') then "public"
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "public"
    let $name := $a/@Name
    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/forms/" || $name || "/controls/" || $datasource/@Name ||  "/methods/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case(data($a/@Package))}</Package>
        <Name name='Name'>{lower-case($m/@Name)}</Name>
        <Kind name="Kind">Method</Kind>
        <IsAbstract name='IsAbstract:Boolean'>{lower-case($m/@IsAbstract)}</IsAbstract>
        <IsStatic name='IsStatic:Boolean'>{lower-case($m/@IsStatic)}</IsStatic>
        <IsFinal name='IsFinal:Boolean'> {lower-case($m/@IsFinal)}</IsFinal>
        <Visibility name='Visibility'>{string($visibility)}</Visibility>
        <CMP name='CMP:int'>{local:MethodComplexity($m)}</CMP>
        <StartLine name='StartLine:int'>{xs:integer($m/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($m/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($m/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($m/@EndCol)}</EndCol>
        <LOC name='LOC:int'>{$m/@EndLine - $m/@StartLine + 1}</LOC>
        <NOS name='NOS:int'>{count(for $stmt in $m//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
        <Label name=':LABEL'>Method</Label>        
     </Record>
}
</MethodsOnForms>

return csv:serialize($r, $options)
