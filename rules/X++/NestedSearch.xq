(: Finds x++ nested "while select" statements  :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/NestedSearch' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a/Method 
  for $nested in $m//(SearchStatement | DoWhileStatement | ForStatement | WhileStatement)//SearchStatement  
  return
    <Diagnostic>
      <Moniker>NestedSearch</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method contains a nested "while select" statement. Whenever possible, use a join in the select statement, rather than using an inner while loop on the related table.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($nested/@StartLine)}</Line>
      <Column>{string($nested/@StartCol)}</Column>
      <EndLine>{string($nested/@EndLine)}</EndLine>
      <EndColumn>{string($nested/@EndCol)}</EndColumn>
    </Diagnostic>  
}
</Diagnostics>
