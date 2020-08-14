(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information on methods on forms datasources in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnFormDataFields>
{
    for $a in /Form
    for $datasource in $a/FormDataSource
    for $field in $datasource/FormDataField
    for $m in $field/Method
    return <Record>
        <From name=":START_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name || "/datafields/" || $field/@Name) }</From>
        <To name=":END_ID">    {lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name || "/datafields/" || $field/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnFormDataFields>

return csv:serialize($r, $options)
