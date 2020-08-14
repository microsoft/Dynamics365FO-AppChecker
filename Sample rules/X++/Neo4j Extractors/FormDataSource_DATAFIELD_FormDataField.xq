(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information on methods on forms datasources in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnForms>
{
    for $a in /Form
    for $datasource in $a/FormDataSource
    for $field in $datasource/FormDataField


    return <Record>
        <From name=":START_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name) }</From>
        <To name=":END_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name || "/datafields/" || $field/@Name) }</To>
        <Type name=':TYPE'>FIELD</Type>
     </Record>

}
</MethodsOnForms>

return csv:serialize($r, $options)
