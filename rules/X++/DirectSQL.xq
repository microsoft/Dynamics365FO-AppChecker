(: Finds references To Direct SQL in x++ code :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/DirectSQL' Version='1.0'>
{
  for $a in /*
  for $m in $a/Method 
  for $qc in $m//QualifiedCall
  where $qc/ExpressionQualifier[@Type = "Statement"] 
  and $qc[@MethodName = "executeUpdate" or @MethodName = "executeQuery"]
  return
    <Diagnostic>
      <Moniker>DirectSQL</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method contains direct SQL code. This should be avoided for security reasons, and because of the maintenance cost and possible issues when the database objects change.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($qc/@StartLine)}</Line>
      <Column>{string($qc/@StartCol)}</Column>
      <EndLine>{string($qc/@EndLine)}</EndLine>
      <EndColumn>{string($qc/@EndCol)}</EndColumn>
    </Diagnostic>  
}
</Diagnostics>
