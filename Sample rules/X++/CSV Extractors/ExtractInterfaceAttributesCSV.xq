(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnInterfaces>
{
    for $a in /Interface
    order by $a/@Name
    for $attr in $a/AttributeList/Attribute
    return <Record>
        <Interface>{lower-case($a/@Name)}</Interface>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnInterfaces>

return csv:serialize($r, $options)