(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information about class inheritance in CSV :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $classes := <Classes>
{
    for $c in /Class
    return <Class Name='{$c/@Name}' Extends='{$c/@Extends}' Package='{$c/@Package}' />
}
</Classes>

let $r := <ClassExtension>
{
    for $c in $classes/Class[@Extends != '']

    (: Include only if the extending class is in the database :)
    for $e in $classes/Class[lower-case(@Name)=lower-case($c/@Extends)][position() = 1]
    return <Record>
       <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name) }</From>
       <To name=':END_ID'>{ lower-case("/" || $e/@Package || "/classes/" || $c/@Extends) }</To>
       <Type name=':TYPE'>EXTENDS</Type>
    </Record>
}
</ClassExtension>

return csv:serialize($r, $options)
