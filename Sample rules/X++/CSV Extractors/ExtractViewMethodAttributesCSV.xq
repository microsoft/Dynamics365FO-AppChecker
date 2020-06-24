(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes applied on tables in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnViews>
{
    for $a in /View
    order by $a/@Name
    for $m in $a/Method
    for $attr in $m/AttributeList/Attribute
    return <Record>
        <Package>{lower-case($a/@Package)}</Package>
        <View>{lower-case($a/@Name)}</View>
        <Method>{lower-case($m/@Name)}</Method>
        <Attribute>{lower-case($attr/@Name)}</Attribute>
    </Record>
}
</AttributesOnViews>

return csv:serialize($r, $options)
