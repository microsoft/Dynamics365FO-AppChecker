(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract class method information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <MethodsOnClasses>
{
    for $a in /Class
    for $m in $a/Delegate

    return <Record>
        <Artifact name='Artifact:ID'>{ lower-case("/" || $a/@Package || "/classes/" || $a/@Name || "/delegates/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Class name='Class'>{lower-case($a/@Name)}</Class>
        <Delegate name='Name'>{lower-case($m/@Name)}</Delegate>
        <Kind name="Kind">Delegate</Kind>
        <Label name=':LABEL'>Delegate</Label>
     </Record>
}
</MethodsOnClasses>

return csv:serialize($r, $options)
