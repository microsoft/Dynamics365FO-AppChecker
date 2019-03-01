(: Finds use of breakpoint statement :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/Breakpoint' Version='1.0'>
{
  for $a in /*
  for $m in $a//Method
  for $s in $m//BreakpointStatement
  return
    <Diagnostic>
      <Moniker>Breakpoint</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method contains a breakpoint statement.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($s/@StartLine)}</Line>
      <Column>{string($s/@StartCol)}</Column>
      <EndLine>{string($s/@EndLine)}</EndLine>
      <EndColumn>{string($s/@EndCol)}</EndColumn>
    </Diagnostic>  
} 
</Diagnostics>
