(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export view method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnViews>
{
    for $a in /View
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/views/" || $a/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/@Package || "/views/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnViews>

return csv:serialize($r, $options)
