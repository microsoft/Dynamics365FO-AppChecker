(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract class extension information as prolog clauses :)

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

for $c in /Class[lower-case(@Package) = $packages]
where $c/@Extends != ''
return "classextends(c('" || lower-case($c/@Name) || "'), i('" || lower-case($c/@Extends) ||  "'))."

