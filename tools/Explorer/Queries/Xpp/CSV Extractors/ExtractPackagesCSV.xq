(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract packages and render result in CSV format :)

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
    return <Record>
       <Package>{lower-case($m/@Name)}</Package>
       <ModelId>{lower-case($m/@Id)}</ModelId>
       <Description>{normalize-space(lower-case($m/@Description))}</Description>
    </Record>
}
</Packages>

return csv:serialize($r, $options)
