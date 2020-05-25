(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract some tables metadata as prolog clauses :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

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
 
for $t in /Table[lower-case(@Package)=$packages]
let $systemTableValue := if ($t/Metadata/SystemTable) then $t/Metadata/SystemTable else "no"
let $saveDataPerPartition := if ($t/Metadata/SaveDataPerPartition) then $t/Metadata/SaveDataPerPartition else "yes"
return "tabledef(t('" || lower-case(lower-case($t/@Name)) || "'), p('" || lower-case($t/@Package) || "' ), "
        || "{ systemTable: " || (if (lower-case($systemTableValue) = "yes") then "true" else "false" )
        || ", savedataperpartition: " || (if (lower-case($systemTableValue) = "yes") then "true" else "false" )
        || (if ($t/Metadata/ClusteredIndex != "") then 
             ", clusteredindex: '" || lower-case($t/Metadata/ClusteredIndex) || "'"
            else "") 
        || (if ($t/Metadata/TableGroup != "") then 
             ", tablegroup: '" || lower-case($t/Metadata/TableGroup) || "'"
            else "") 
        || "}"
        || ")."
