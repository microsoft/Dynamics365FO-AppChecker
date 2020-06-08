(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract query data sources in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <QueryDataSources>
{
    for $a in /Query
    for $ds in $a/Metadata/DataSources//Table
    return <Record>
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Table>{$ds/data()}</Table>
     </Record>
}
</QueryDataSources>

return csv:serialize($r, $options)