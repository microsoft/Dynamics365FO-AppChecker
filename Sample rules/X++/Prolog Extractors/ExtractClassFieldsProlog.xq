(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Fields (Members) in classes :)

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

for $a in /Class[lower-case(@Package)=$packages]
for $m in $a/FieldDeclaration
let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private" 
                else if (lower-case($m/@IsProtected) = 'true') then 'protected' 
                else if (lower-case($m/@IsPublic) = 'true') then "public" 
                else if (lower-case($m/@IsInternal) = 'true') then "internal"
                else "protected"

return "classfield(c('" || lower-case($a/@Name) || "'),"
               || "f('" || lower-case($m/@Name) || "', '" || lower-case($m/@Type) || "', " || $visibility || "))."
