(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export form information in CSV format. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <FormsDatasources>
{
    for $a in /Form
    for $datasource in $a/FormDataSource

    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name)}</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Name name="Name">{lower-case($a/@Name)}</Name>
        <Kind name="Kind">FormDataSource</Kind>
        <Label name=':LABEL'>FormDataSource</Label>        
     </Record>
}
</FormsDatasources>

return csv:serialize($r, $options)
