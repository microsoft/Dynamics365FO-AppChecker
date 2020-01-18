(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Interface inheritance information as prolog clauses :)

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
   
for $a in /Interface[lower-case(@Package)=$packages]
where $a/@Extends != ""
return "extends(i('" || lower-case($a/@Name) || "'), i('" || lower-case($a/@Extends) || "'))."
