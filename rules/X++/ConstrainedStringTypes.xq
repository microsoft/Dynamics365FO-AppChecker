(: Find References to all constrained string types :)
(: @Language Xpp :)
(: @Category Mandatory :)
(: @Author pvillads@microsoft.com :)

<ConstrainedStringTypes Category='Mandatory' href='docs.microsoft.com/Socratex/ConstrainedStringTypes' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $s in $m//StringLengthType
  return 
    <Diagnostic>
      <Moniker>ConstrainedStringTypeReference</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>References to hardcoded string lengths are not allowed, since it precludes customizations requiring recompilations.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($s/@StartLine)}</Line>
      <Column>{string($s/@StartCol)}</Column>
      <EndLine>{string($s/@EndLine)}</EndLine>
      <EndColumn>{string($s/@EndCol)}</EndColumn>
    </Diagnostic> 
 }
</ConstrainedStringTypes>
