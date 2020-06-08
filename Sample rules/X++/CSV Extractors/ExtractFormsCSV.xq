(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract form information in CSV format. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <FormsMetrics>
{
    for $a in /Form
    return <Record>
        <Package>{lower-case($a/@Package)}</Package>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Name>{lower-case($a/@Name)}</Name>
     </Record>
}
</FormsMetrics>

return csv:serialize($r, $options)
