(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about delegates on classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <DelegatesOnClasses>
{
    for $a in /Class
    for $m in $a/Delegate
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Name>{lower-case($m/@Name)}</Name>
    </Record>
}
</DelegatesOnClasses>

return csv:serialize($r, $options)
