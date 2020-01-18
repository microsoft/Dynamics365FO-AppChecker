(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract package dependencies in CSV format :)

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
        
let $r := <Packages>
{
    for $m in /Models/Model
    for $ref in $m/ModuleReferences/ModuleReference
    return <Record>
       <Package>{lower-case($m/@Name)}</Package>
       <References>{lower-case($ref/@Name)}</References>
    </Record>
}
</Packages>

return csv:serialize($r, $options)
