(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes applied on forms in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <AttributesOnForms>
{
    for $a in /Form
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
