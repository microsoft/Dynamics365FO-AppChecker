(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract some views metadata in CSV format :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <Views>
{
    for $t in /View

    return <Record>
        <Package>{lower-case($t/@Package)}</Package>
        <Artifact>{lower-case($t/@Artifact)}</Artifact>
        <Name>{lower-case(lower-case($t/@Name))}</Name>
        <Label>{lower-case(lower-case($t/Metadata/Label))}</Label>
        <TableGroup>{lower-case($t/Metadata/TableGroup)}</TableGroup>
    </Record>
}
</Views>

return csv:serialize($r, $options)
