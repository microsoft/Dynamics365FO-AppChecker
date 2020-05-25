(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about the interfaces a class implements as prolog clauses :)

let $packages := ('', 'applicationfoundation', '', '', 'applicationcommon',
        'directory', 'calendar', 'dimensions', 'currency',
        'unitofmeasure', 'measurement', 'sourcedocumentationtypes', 'sourcedocumentation',
        'ledger', 'electronicreportingdotnetutils', 'contactperson', 'datasharing',
        'policy', 'electronicreportingcore', 'banktypes', 'project', 
        'electronicreportingmapping', 'tax', 'subledger', 'personnelcore',
        'electronicreportingforax', 'businessprocess', 'casemanagement', 'generalledger',
        'electronicreporting', 'personnelmanagement', 'financialreporting', 'fiscalbooks',
        'taxengine', 'electronicreportingbusinessdoc', 'personnel', 'retail',
        'applicationsuite')
 
for $c in /Class[lower-case(@Package)=$packages]
for $i in $c/Implements
return "classimplements(c('" || lower-case($c/@Name) || "'), i('" || lower-case($i/text()) || "'))."
