(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract some query metadata in CSV format :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

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
        
let $r := <Queries>
{
    for $t in /Query[lower-case(@Package)=$packages]    
    return <Record>
        <Package>{lower-case($t/@Package)}</Package>
        <Artifact>{lower-case($t/@Artifact)}</Artifact>
        <Name>{lower-case($t/@Name)}</Name>
        <Title>{lower-case($t/Metadata/Title/data())}</Title>
    </Record>      
}
</Queries>

return csv:serialize($r, $options)
