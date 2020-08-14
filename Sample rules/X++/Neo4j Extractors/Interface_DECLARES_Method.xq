(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export interface method mapping in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnInterfaces>
{
    for $a in /Interface
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/interfaces/" || $a/@Name) }</From>
        <To name=':END_ID'>{  lower-case("/" || $a/@Package || "/interfaces/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnInterfaces>

return csv:serialize($r, $options)
