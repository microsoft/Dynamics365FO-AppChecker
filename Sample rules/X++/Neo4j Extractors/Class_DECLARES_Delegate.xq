(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information about delegates on classes in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <DelegatesOnClasses>
{
    for $c in /Class
    for $m in $c/Delegate
    return <Record>
       <From name=':START_ID'>{lower-case("/" || $c/@Package || "/classes/" || $c/@Name) }</From>
       <To name=':END_ID'>{lower-case("/" || $c/@Package || "/classes/" || $c/@Name || "/delegates/" || $m/@Name) }</To>
       <Type name=':TYPE'>DECLARES</Type>
    </Record>
}
</DelegatesOnClasses>

return csv:serialize($r, $options)
