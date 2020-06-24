(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract interface method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <MethodsOnInterfaces>
{
    for $a in /Interface
    for $m in $a/Method
    return <Record>
        <Package>{lower-case($a/@Package)}</Package>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Method>{lower-case($m/@Name)}</Method>
     </Record>
}
</MethodsOnInterfaces>

return csv:serialize($r, $options)
