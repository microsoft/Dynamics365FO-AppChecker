(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get the list of tables and the fields that have an asset classification :)
(: @Category Informational :)
(: @Language Xpp :)

<TablesWithAssetClassificationFields>
{
    for $t in /Table
    order by $t/@Package, $t/@Name
    for $f in $t/Metadata/Fields/AxTableField
    where $f/AssetClassification
    return <TablesWithAssetClassificationField Package='{$t/@Package}' Table='{$t/@Name}' Field='{$f/Name}' AssetClassification='{$f/AssetClassification}'/>
}
</TablesWithAssetClassificationFields>
