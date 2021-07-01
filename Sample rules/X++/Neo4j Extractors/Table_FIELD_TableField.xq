(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export table field information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <FieldsOnTables>
{
    for $a in /Table
    for $f in $a/Metadata/Fields/AxTableField
    
    return <Record>
        <From name=":START_ID">{lower-case("/" || $a/@Package || "/tables/" || $a/@Name) }</From>
        <To name=":END_ID">{    lower-case("/" || $a/@Package || "/tables/" || $a/@Name || "/fields/" || $f/Name)}</To>
        <Type name=':TYPE'>FIELD</Type>
     </Record>
}
</FieldsOnTables>

return csv:serialize($r, $options)
