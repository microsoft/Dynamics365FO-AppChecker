(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract query data sources in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $tables := <Tables>
{
    for $t in /Table
    return <Table Name='{lower-case($t/@Name)}' Package='{lower-case($t/@Package)}' />
}
</Tables>

let $r := <QueryDataSources>
{
    for $a in /Query
    for $ds in $a/Metadata/DataSources//Table/data()
    for $t in $tables/Table[lower-case(@Name)=lower-case($ds)][position()=1]

    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $a/@Package || "/queries/" || $a/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $t/@Package || "/tables/" || $t/@Name) }</To>
        <Type name=':TYPE'>REFERENCES</Type>
     </Record>
}
</QueryDataSources>

return csv:serialize($r, $options)