(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information about class implementing fields in CSV :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <ClassFieldDeclaration>
{
    for $c in /Class
    for $i in $c/FieldDeclaration
    return <Record>
       <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name) }</From>
       <To name=':END_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name || "/fields/" || $i/@Name) }</To>
       <Type name=':TYPE'>DECLARES</Type>
    </Record>
}
</ClassFieldDeclaration>

return csv:serialize($r, $options)
