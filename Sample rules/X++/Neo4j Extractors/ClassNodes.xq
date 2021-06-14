(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide interesting metrics for classes. The metrics provided
follow the naming conventions used in the literature:

IsAbstract: Whether or not the class is abstract.
IsInterface: Whether or not the name is an interface (i.e. not a class).
Extends: The parent class of this class, if any.
NOAM: The number of abstract methods.
LOC: Lines of Code.
NOS: Number of statements.
NOM: Number of methods.
NOPM: The number of private methods.
NOA (Number of Attributes): The number of fields.
NOPA: Number of public fields.
WMC: Weighted Method Complexity, the sum of the method complexities across all methods.
:)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <ClassMetrics>
{
    for $a in /Class
    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))
    return <Record>
        <Artifact name="Artifact:ID">{ lower-case("/" || $a/@Package || "/classes/" || $a/@Name)}</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Name name="Name">{lower-case($a/@Name)}</Name>
        <Kind name="Kind">Class</Kind>
        <IsAbstract name="IsAbstract:Boolean">{lower-case($a/@IsAbstract)}</IsAbstract>
        <IsFinal name="IsFinal:Boolean">{lower-case($a/@IsFinal)}</IsFinal>
        <IsStatic name="IsStatic:Boolean">{lower-case($a/@IsStatic)}</IsStatic>
        <NOAM name='NOAM:int'>{count($a/Method[@IsAbstract="true"])}</NOAM>
        <Source name='base64Source'>{string(convert:string-to-base64(data($a/@Source)))}</Source>
        <Comments name='base64Comments'>{string(convert:string-to-base64(data($a/@Comments)))}</Comments>
        <LOC name='LOC:int'>{$a/@EndLine - $a/@StartLine + 1}</LOC>
        <NOM name='NOM:int'>{count($a/Method)}</NOM>
        <NOA name='NOA:int'>{count($a/FieldDeclaration)}</NOA>
        <WMC name='WMC:int'>{$weightedMethodComplexity}</WMC>
        <NOPM name='NOPM:int'>{count($a/Method[lower-case(@IsPublic)="true"])}</NOPM>
        <NOPA name='NOPA:int'>{count($a/FieldDeclaration[lower-case(@IsPublic)="true"])}</NOPA>
        <NOS name='NOS:int'>{count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
        <Label name=':LABEL'>Class</Label>
     </Record>
}
</ClassMetrics>

return csv:serialize($r, $options)
