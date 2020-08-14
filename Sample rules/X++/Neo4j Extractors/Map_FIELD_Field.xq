(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export map field information :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MapFields>
{
    for $t in /Map
    for $f in $t/Metadata/Fields/AxMapBaseField

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $t/@Package || "/maps/" || $t/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $t/@Package || "/maps/" || $t/@Name || "/fields/" || $f/Name) }</To>
        <Type name=':TYPE'>FIELD</Type>
    </Record>
}
</MapFields>

return csv:serialize($r, $options)