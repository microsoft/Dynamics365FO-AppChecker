(: Check for single delete statement in search statement, on the search query buffer :)
(: @Language Xpp :)

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/DeleteFromNotUsed' Version='1.0'>
{
  for $a in /*
  for $m in $a//Method
  for $s in $m//SearchStatement
  where count($s/CompoundStatement/*) = 1
  let $bn := string($s/Query/@BufferName)
  for $qc in $s/CompoundStatement/ExpressionStatement/QualifiedCall[@MethodName = "delete"]
  for $sq in $qc/SimpleQualifier
  where string($sq/@Name) = $bn
  return
    <Diagnostic>
      <Moniker>DeleteFromNotUsed</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>Validate if delete_from can be used for this operation. Use set based instead of row based database operations when possible.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($qc/@StartLine)}</Line>
      <Column>{string($qc/@StartCol)}</Column>
      <EndLine>{string($qc/@EndLine)}</EndLine>
      <EndColumn>{string($qc/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>