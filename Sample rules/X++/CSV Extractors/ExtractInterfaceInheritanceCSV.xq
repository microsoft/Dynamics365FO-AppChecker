(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Interface inheritance information in CSV format :)

let $options := map { 'lax': false(), 'header': true() }
let $packages := ('', 'applicationfoundation', 'applicationplatform', '', 'applicationcommon',
        'directory', 'calendar', 'dimensions', 'currency',
        'unitofmeasure', 'measurement', 'sourcedocumentationtypes', 'sourcedocumentation',
        'ledger', 'electronicreportingdotnetutils', 'contactperson', 'datasharing',
        'policy', 'electronicreportingcore', 'banktypes', 'project', 
        'electronicreportingmapping', 'tax', 'subledger', 'personnelcore',
        'electronicreportingforax', 'businessprocess', 'casemanagement', 'generalledger',
        'electronicreporting', 'personnelmanagement', 'financialreporting', 'fiscalbooks',
        'taxengine', 'electronicreportingbusinessdoc', 'personnel', 'retail',
        'applicationsuite')
        
let $r := <InterfaceInheritance>
{
    for $a in /Interface[lower-case(@Package)=$packages]
    where $a/@Extends != ""
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Extends>{lower-case($a/@Extends)}</Extends>
     </Record>
}
</InterfaceInheritance>

return csv:serialize($r, $options)
