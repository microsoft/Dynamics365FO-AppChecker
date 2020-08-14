(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export query method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnQueries>
{
    for $a in /Query/Class
    for $m in $a/Method
    let $name := tokenize($a/@Name, "\$")[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/../@Package || "/queries/" || $name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/../@Package || "/queries/" || $name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnQueries>

return csv:serialize($r, $options)
