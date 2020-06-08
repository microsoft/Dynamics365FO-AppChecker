(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes applied on classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnClasses>
{
    for $a in /Class
    order by $a/@Name
    for $m in $a/Method
    for $attr in $m/AttributeList/Attribute
    return <Record>
        <Class>{lower-case($a/@Name)}</Class>
        <Method>{lower-case($m/@Name)}</Method>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnClasses>

return csv:serialize($r, $options)
