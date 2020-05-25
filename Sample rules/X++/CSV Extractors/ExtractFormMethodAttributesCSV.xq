(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
  
(: Extract information about attributes applied on forms in CSV format :)

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

let $r := <AttributesOnForms>
{
    for $a in /Form[lower-case(@Package)=$packages]
    order by $a/@Name
    for $m in $a/Class/Method
    for $attr in $m/AttributeList/Attribute
    return <Record>
        <Form>{lower-case($a/@Name)}</Form>
        <Method>{lower-case($m/@Name)}</Method>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnForms>

return csv:serialize($r, $options)
