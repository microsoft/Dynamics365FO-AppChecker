(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

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

let $r := <AttributesOnInterfaces>
{
    for $a in /Interface[lower-case(@Package)=$packages]
    order by $a/@Name
    for $attr in $a/AttributeList/Attribute
    return <Record>
        <Interface>{lower-case($a/@Name)}</Interface>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnInterfaces>

return csv:serialize($r, $options)