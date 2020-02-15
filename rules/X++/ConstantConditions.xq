(: Find constant conditions in conditional statements :)
(: @Language Xpp :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/ConstantConditions' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $s in $m//(IfStatement | IfThenElseStatement | WhileStatement)/*[1][lower-case(@IsConst)='true']
  return
    <Diagnostic>
      <Moniker>ConstantCondition</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method contains a constant condition.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($s/@StartLine)}</Line>
      <Column>{string($s/@StartCol)}</Column>
      <EndLine>{string($s/@EndLine)}</EndLine>
      <EndColumn>{string($s/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>
