(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export act class method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnClasses>
{
    for $a in /Class
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/classes/" || $a/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $a/@Package || "/classes/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnClasses>

return csv:serialize($r, $options)
