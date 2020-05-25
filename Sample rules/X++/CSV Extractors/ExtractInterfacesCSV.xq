(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Interface information in CSV format :)

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
        
let $r := <Interfaces>
{
    for $a in /Interface[lower-case(@Package)=$packages]
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
		<Name>{lower-case($a/@Name)}</Name>
        <Package>{lower-case($a/@Package)}</Package>
     </Record>
}
</Interfaces>

return csv:serialize($r, $options)
