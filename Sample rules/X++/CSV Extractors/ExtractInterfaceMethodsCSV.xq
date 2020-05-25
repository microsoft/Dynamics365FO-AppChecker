(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract interface method information in CSV format :)

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

let $r := <MethodsOnInterfaces>
{
    for $a in /Interface[lower-case(@Package)=$packages]
    for $m in $a/Method
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Method>{lower-case($m/@Name)}</Method>
     </Record>
}
</MethodsOnInterfaces>

return csv:serialize($r, $options)
