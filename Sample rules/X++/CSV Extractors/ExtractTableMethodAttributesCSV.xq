(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes applied on tables in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnTables>
{
    for $a in /Table
    order by $a/@Name
    for $m in $a/Method
    for $attr in $m/AttributeList/Attribute
    return <Record>
        <Table>{lower-case($a/@Name)}</Table>
        <Method>{lower-case($m/@Name)}</Method>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnTables>

return csv:serialize($r, $options)
