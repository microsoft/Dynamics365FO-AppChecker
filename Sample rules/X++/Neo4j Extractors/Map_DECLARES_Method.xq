(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export map method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnMaps>
{
    for $a in /Map
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/maps/" || $a/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/@Package || "/maps/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnMaps>

return csv:serialize($r, $options)
