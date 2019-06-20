(: Find classes that implement IDisposable, but do not have a Dispose method :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/IDisposableWithoutDispose' Version='1.0'>
{
  for $c in /Class
  for $i in $c/Implements[.='System.IDisposable']
  where not($c/Method[lower-case(@Name)='dispose'])
  return
    <Diagnostic Artifact='{$c/@Artifact}'>
      <Moniker>IDisposableWithoutDispose</Moniker>
      <Severity>Error</Severity>
      <Path>{string($c/@PathPrefix)}</Path>
      <Message>This class implements IDisposable but does not include a Dispose() method.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($c/@StartLine)}</Line>
      <Column>{string($c/@StartCol)}</Column>
      <EndLine>{string($c/@EndLine)}</EndLine>
      <EndColumn>{string($c/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>