(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about delegates on classes in CSV format :)

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

let $r := <DelegatesOnClasses>
{
    for $a in /Class[lower-case(@Package)=$packages]
    for $m in $a/Delegate
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Name>{lower-case($m/@Name)}</Name>
    </Record>
}
</DelegatesOnClasses>

return csv:serialize($r, $options)
