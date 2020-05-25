(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract query data sources in CSV format :)

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

let $r := <QueryDataSources>
{
    for $a in /Query[lower-case(@Package)=$packages]
    for $ds in $a/Metadata/DataSources//Table
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Table>{$ds/data()}</Table>
     </Record>
}
</QueryDataSources>

return csv:serialize($r, $options)