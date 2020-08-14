(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export View field information :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <ViewFields>
{
    for $t in /View
    for $f in $t/Metadata/Fields/AxViewField

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $t/@Package || "/views/" || $t/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $t/@Package || "/views/" || $t/@Name || "/fields/" || $f/Name) }</To>
        <Type name=':TYPE'>FIELD</Type>
    </Record>
}
</ViewFields>

return csv:serialize($r, $options)