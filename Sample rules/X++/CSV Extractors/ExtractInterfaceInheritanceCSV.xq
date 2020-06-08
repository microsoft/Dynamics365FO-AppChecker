(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Interface inheritance information in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <InterfaceInheritance>
{
    for $a in /Interface
    where $a/@Extends != ""
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Extends>{lower-case($a/@Extends)}</Extends>
     </Record>
}
</InterfaceInheritance>

return csv:serialize($r, $options)
