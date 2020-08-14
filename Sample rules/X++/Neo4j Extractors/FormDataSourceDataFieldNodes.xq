(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export information on methods on forms datasources in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <MethodsOnForms>
{
    for $a in /Form
    for $datasource in $a/FormDataSource
    for $field in $datasource/FormDataField
    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/datasources/" || $datasource/@Name || "/datafields/" || $field/@Name) }</Artifact>
        <Package name='Package'>{lower-case(data($a/@Package))}</Package>
        <Name name='Name'>{lower-case($field/@Name)}</Name>
        <Kind name="Kind">FormDataField</Kind>
        <Label name=':LABEL'>FormDataField</Label>
    </Record>
}
</MethodsOnForms>

return csv:serialize($r, $options)
