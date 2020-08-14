(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information about interface inheritance in CSV :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <Extension>
{
    for $c in /Interface[@Extends != '']

    (: Only proceed if extended interface is defined in the database :)
    for $e in /Interface[lower-case(@Name)=lower-case($c/@Extends)][position()=1]

    return <Record>
       <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/interfaces/" || $c/@Name) }</From>
       <To name=':END_ID'>{ lower-case("/" || $e/@Package || "/interfaces/" || $e/@Name) }</To>
       <Type name=':TYPE'>EXTENDS</Type>
    </Record>
}
</Extension>

return csv:serialize($r, $options)
