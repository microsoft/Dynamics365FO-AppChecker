(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about class extension in CSV :)

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

let $r := <Extension>
{
    for $c in /Class[lower-case(@Package) = $packages]
    where $c/@Extends != ''
    return <Record>
       <Artifact>{lower-case($c/@Artifact)}</Artifact>
       <Extends>{lower-case($c/@Extends)}</Extends>
    </Record>
}
</Extension>

return csv:serialize($r, $options)
