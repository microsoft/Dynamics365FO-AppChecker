(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export forms method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnForms>
{
    for $a in /Form/Class
    for $m in $a/Method
    let $name := tokenize($a/@Name, "\$")[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/../@Package || "/forms/" || $name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/../@Package || "/forms/" || $name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnForms>

return csv:serialize($r, $options)
