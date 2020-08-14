(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export entity method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnEntities>
{
    for $a in /Entity
    for $m in $a/Method

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/dataentityviews/" || $a/@Name) }</From>
        <To name=':END_ID'    >{ lower-case("/" || $a/@Package || "/dataentityviews/" || $a/@Name || "/methods/" || $m/@Name) }</To>
        <Type name=':TYPE'>DECLARES</Type>
     </Record>
}
</MethodsOnEntities>

return csv:serialize($r, $options)
