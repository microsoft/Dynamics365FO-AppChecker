(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export table method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnTables>
{
    for $a in /Table
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/tables/" || $a/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/@Package || "/tables/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnTables>

return csv:serialize($r, $options)
