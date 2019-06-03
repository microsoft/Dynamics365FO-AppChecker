(: Finds tables with no cluster index :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/TableIndex' Version='1.0'>
{
  for $t in /Table
  where exists ($t//ClusteredIndex)
  and $t//ClusteredIndex = ''
  return
    <Diagnostic>
      <Moniker>TableIndex</Moniker>
      <Severity>Error</Severity>
      <Path>{string($t/@PathPrefix)}</Path>
      <Message>All tables should have a clustered index.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($t/@StartLine)}</Line>
      <Column>{string($t/@StartCol)}</Column>
      <EndLine>{string($t/@EndLine)}</EndLine>
      <EndColumn>{string($t/@EndCol)}</EndColumn>      
    </Diagnostic>  
} 
</Diagnostics>
