(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information on methods on queries in CSV format :)
declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

let $options := map { 'lax': false(), 'header': true() }
let $packages := ('applicationfoundation', 'applicationplatform', '', 'applicationcommon',
        'directory', 'calendar', 'dimensions', 'currency',
        'unitofmeasure', 'measurement', 'sourcedocumentationtypes', 'sourcedocumentation',
        'ledger', 'electronicreportingdotnetutils', 'contactperson', 'datasharing',
        'policy', 'electronicreportingcore', 'banktypes', 'project', 
        'electronicreportingmapping', 'tax', 'subledger', 'personnelcore',
        'electronicreportingforax', 'businessprocess', 'casemanagement', 'generalledger',
        'electronicreporting', 'personnelmanagement', 'financialreporting', 'fiscalbooks',
        'taxengine', 'electronicreportingbusinessdoc', 'personnel', 'retail',
        'applicationsuite')

let $r := <MethodsOnQueries>
{
    for $a in /Query[lower-case(@Package)=$packages]/Class
    for $m in $a/Method
    let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private" 
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected' 
                  else if (lower-case($m/@IsPublic) = 'true') then "public" 
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "public"
    return <Record>
        <Artifact>{lower-case($a/../@Artifact)}</Artifact>
        <Name>{lower-case($m/@Name)}</Name>
        <IsAbstract>{lower-case($m/@IsAbstract)}</IsAbstract>
		<IsStatic>{lower-case($m/@IsStatic)}</IsStatic>
		<IsFinal>{lower-case($m/@IsFinal)}</IsFinal>
        <Visibility>{string($visibility)}</Visibility>
        <CMP>{local:MethodComplexity($m)}</CMP>
        <LOC>{$m/@EndLine - $m/@StartLine + 1}</LOC>
        <NOS>{count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
     </Record>
}
</MethodsOnQueries>

return csv:serialize($r, $options)
