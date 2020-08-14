(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export form information in CSV format. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <FormControls>
{
    for $a in /Form
    for $control in $a/FormControl

    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/controls/" || $control/@Name)}</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Name name="Name">{lower-case($a/@Name)}</Name>
        <Kind name="Kind">FormControl</Kind>
        <Label name=':LABEL'>FormControl</Label>
     </Record>
}
</FormControls>

return csv:serialize($r, $options)
