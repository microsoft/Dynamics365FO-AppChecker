<TableDefinitions>
{
    for $cts in //CreateTableStatement
    return <TableDefinition Name='{$cts/SchemaObjectName/Identifier/@Value}'>
    {
        for $cd in $cts/TableDefinition/ColumnDefinition
        return <Column Name='{$cd/Identifier/@Value}' Type='{$cd/SqlDataTypeReference//Identifier/@Value}'/>
    }
    </TableDefinition>
}
</TableDefinitions>