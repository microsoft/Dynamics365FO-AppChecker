(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Table field information as prolog clauses :)

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

for $t in /Table[lower-case(@Package) = $packages]
for $f in $t/Metadata/Fields/AxTableField 
let $mandatory := if ($f/Mandatory) then "true" else "false"
let $visible := if ($f/Visible) then "true" else "false"
let $edt := if ($f/ExtendedDataType) then $f/ExtendedDataType else ""
let $label := if ($f/Label) then $f/Label else ""
return "tablefield(t('" || lower-case($t/@Name) || "'), f('" || lower-case($f/Name) || "'), " 
       || lower-case($f/@Q{http://www.w3.org/2001/XMLSchema-instance}type)  || ", " 
       || "{" 
       ||    " mandatory: " || lower-case($mandatory) 
       ||   ", visible: "   || lower-case($visible)
       ||   (if ($edt) then ", edt: " || lower-case($edt) else "") 
       || "}"
       || ")."
