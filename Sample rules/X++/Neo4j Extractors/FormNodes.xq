(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export form information in CSV format. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <FormsMetrics>
{
    for $a in /Form/Class

    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))

    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/../@Package || "/forms/" || $a/../@Name)}</Artifact>
        <Package name='Package'>{lower-case($a/../@Package)}</Package>
        <Name name="Name">{lower-case($a/../@Name)}</Name>
        <Kind name="Kind">Form</Kind>
        <Source name='base64Source'>{string(convert:string-to-base64(data($a/../@Source)))}</Source>
        <Comments name='base64Comments'>{string(convert:string-to-base64(data($a/../@Comments)))}</Comments>
        <LOC name='LOC'>{ $a/@EndLine - $a/@StartLine + 1}</LOC>
        <NOM name='NOM'> {count($a/Method)}</NOM>
        <WMC name='WMC'>{$weightedMethodComplexity}</WMC>
        <NOPM name='NOPM'>{count($a/Method[lower-case(@IsPublic)="true"])}</NOPM>
        <NOS name='NOS'>{count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
        <Label name=':LABEL'>Form</Label>
     </Record>
}
</FormsMetrics>

return csv:serialize($r, $options)
