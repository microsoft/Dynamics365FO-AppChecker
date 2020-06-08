(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Interface information in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <Interfaces>
{
    for $a in /Interface
    return <Record>
        <Package>{lower-case($a/@Package)}</Package>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Name>{lower-case($a/@Name)}</Name>
     </Record>
}
</Interfaces>

return csv:serialize($r, $options)
