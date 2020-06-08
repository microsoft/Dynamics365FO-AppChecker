(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes applied to classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnClasses>
{
    for $a in /Class
    order by $a/@Name
    for $attr in $a/AttributeList/Attribute
    return <Record>
        <Artifact>{lower-case($a/@Name)}</Artifact>
        <Name>{lower-case($attr/@Name)}</Name>
    </Record>
}
</AttributesOnClasses>

return csv:serialize($r, $options)
