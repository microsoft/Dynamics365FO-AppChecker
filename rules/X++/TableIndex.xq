(: Finds tables with no cluster index :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/TableIndex' Version='1.0'>
{
  for $t in /Table
  where not (exists ($t//ClusteredIndex))
  let $typeNamePair := fn:tokenize($t/@Artifact, ":")  
  return
    <Diagnostic>
      <Moniker>TableIndex</Moniker>
      <Severity>Error</Severity>
      <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}</Path>
      <Message>All tables should have a clustered index.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
    </Diagnostic>  
} 
</Diagnostics>