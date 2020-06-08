(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide interesting metrics for classes. The metrics provided
follow the naming conventions used in the literature. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true() }

let $r := <ClassMetrics>
{
    for $a in /Class
    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))
    order by $weightedMethodComplexity
    return <Record>
        <Package>{lower-case($a/@Package)}</Package>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Name>{lower-case($a/@Name)}</Name>
        <IsAbstract>{lower-case($a/@IsAbstract)}</IsAbstract>
        <IsFinal>{lower-case($a/@IsFinal)}</IsFinal>
        <IsStatic>{lower-case($a/@IsStatic)}</IsStatic>
        <NOAM>{count($a/Method[@IsAbstract="true"])}</NOAM>
        <LOC>{$a/@EndLine - $a/@StartLine + 1}</LOC>
        <NOM>{count($a/Method)}</NOM>
        <NOA>{count($a/FieldDeclaration)}</NOA>
        <WMC>{$weightedMethodComplexity}</WMC>
        <NOPM>{count($a/Method[lower-case(@IsPublic)="true"])}</NOPM>
        <NOPA>{count($a/FieldDeclaration[lower-case(@IsPublic)="true"])}</NOPA>
        <NOS>{count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
     </Record>
}
</ClassMetrics>

return csv:serialize($r, $options)
