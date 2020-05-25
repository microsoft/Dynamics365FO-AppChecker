(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about interfaces inmplemented by classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }
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
        
let $r := <ClassImplementations>
{
  for $c in /Class[lower-case(@Package)=$packages]
  for $i in $c/Implements
  return <Record>
     <Artifact>{lower-case($c/@Artifact)}</Artifact>
     <Implements>{lower-case($i/text())}</Implements>
  </Record>
}
</ClassImplementations>

return csv:serialize($r, $options)
