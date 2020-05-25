(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract some tables metadata in CSV format :)
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
        
let $r := <Tables>
{
    for $t in /Table[lower-case(@Package)=$packages]
    
    let $systemTableValue := if ($t/Metadata/SystemTable) then $t/Metadata/SystemTable else "no"
    let $saveDataPerPartition := if ($t/Metadata/SaveDataPerPartition) then $t/Metadata/SaveDataPerPartition else "yes"

    return <Record>
	    <Package>{lower-case($t/@Package)}</Package>
        <Artifact>{lower-case($t/@Artifact)}</Artifact>
        <Name>{lower-case(lower-case($t/@Name))}</Name>
        <Label>{lower-case(lower-case($t/Metadata/Label))}</Label>
        <SystemTable>{if (lower-case($systemTableValue) = "yes") then "true" else "false"}</SystemTable>
        <SaveDataPerPartition>{if (lower-case($saveDataPerPartition) = "yes") then "true" else "false"}</SaveDataPerPartition>
        <ClusteredIndex>{lower-case($t/Metadata/ClusteredIndex)}</ClusteredIndex>  
        <TableGroup>{lower-case($t/Metadata/TableGroup)}</TableGroup>        
    </Record>      
}
</Tables>

return csv:serialize($r, $options)
