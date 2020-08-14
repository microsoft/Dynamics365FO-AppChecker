(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
(: Export information about class interface implementation in CSV :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $interfaces := <Interfaces>
{
    for $i in /Interface
    return <T Name='{$i/@Name}' Package='{$i/@Package}'></T>
}
</Interfaces>

let $r := <ClassInterfaceImplementation>
{
    for $c in /Class/Implements

    (: Only proceed if the interface is defined in the database :)
    for $e in $interfaces/T[lower-case($c/text())=lower-case(@Name)][position()=1]
    let $classPackage :=  $c/../@Package
    let $interfacePackage := $e/@Package
    return <Record>
       <From name=':START_ID'>{ lower-case("/" || $classPackage || "/classes/" || $c/../@Name) }</From>
       <To name=':END_ID'>{ lower-case("/" || $interfacePackage || "/interfaces/" || $e/@Name) }</To>
       <Type name=':TYPE'>IMPLEMENTS</Type>
    </Record>
}
</ClassInterfaceImplementation>

return csv:serialize($r, $options)