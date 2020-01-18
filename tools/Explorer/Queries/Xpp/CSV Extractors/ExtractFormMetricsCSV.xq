(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide interesting metrics for forms. The metrics provided
follow the naming conventions used in the literature. :)

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
        
let $r := <FormMetrics>
{
    for $a in /Form[lower-case(@Package)=$packages]/Class
    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))
    order by $weightedMethodComplexity
    return <Record>
        <Artifact>{lower-case($a/../@Artifact)}</Artifact>
        <Name>{lower-case($a/@Name)}</Name>
        <Package>{lower-case($a/@Package)}</Package>
        <LOC>{ $a/@EndLine - $a/@StartLine + 1}</LOC>
        <NOM>{count($a/Method)}</NOM>
        <WMC>{$weightedMethodComplexity}</WMC>
        <NOPM>{count($a/Method[lower-case(@IsPublic)="true"])}</NOPM>
        <NOS>{count(for $stmt in $a//Method//* where ends-with(name($stmt), "Statement") return $stmt)}</NOS>
     </Record>
}
</FormMetrics>

return csv:serialize($r, $options)
