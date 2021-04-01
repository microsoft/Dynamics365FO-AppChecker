(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export table fields. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)


declare namespace i='http://www.w3.org/2001/XMLSchema-instance';

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <TableFields>
{
    for $a in /Table
    for $f in $a/Metadata/Fields/AxTableField
    return <Record>
        <Artifact name="Artifact:ID">{lower-case("/" || $a/@Package || "/tables/" || $a/@Name || "/fields/" || $f/Name)}</Artifact>
        <AssetClassification>{data($f/AssetClassification)}</AssetClassification>
        <Type>{substring($f/@i:type, 13)}</Type>
        <ExtendedDataType>{data($f/ExtendedDataType)}</ExtendedDataType>
        <Label name=':LABEL'>TableField</Label>
    </Record>
}
</TableFields>

return csv:serialize($r, $options)


