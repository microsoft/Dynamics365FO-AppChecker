(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about class extension in CSV :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <Extension>
{
    for $c in /Class
    where $c/@Extends != ''
    return <Record>
       <Artifact>{lower-case($c/@Artifact)}</Artifact>
       <Extends>{lower-case($c/@Extends)}</Extends>
    </Record>
}
</Extension>

return csv:serialize($r, $options)
