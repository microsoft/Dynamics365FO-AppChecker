(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export Interface information in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <Interfaces>
{
    for $a in /Interface
    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/interfaces/" || $a/@Name)}</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Name name="Name">{lower-case($a/@Name)}</Name>
        <Kind name="Kind">Interface</Kind>
        <Source name='base64Source'>{string(convert:string-to-base64(data($a/@Source)))}</Source>
        <Comments name='base64Comments'>{string(convert:string-to-base64(data($a/@Comments)))}</Comments>
        <Label name=':LABEL'>Interface</Label>
     </Record>
}
</Interfaces>

return csv:serialize($r, $options)
