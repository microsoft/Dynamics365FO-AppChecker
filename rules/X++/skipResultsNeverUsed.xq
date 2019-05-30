<Diagnostics Category='Best Practice' href='docs.microsoft.com/Socratex/skipResultsNeverUsed' Version='1.0' >
{
  for $a in /(Class | Table | Query | Form)
  for $m in $a//Method
  for $calls in $m//ExpressionStatement/QualifiedCall[
       lower-case(@MethodName)='skipdeleteactions'
    or lower-case(@MethodName)='skipdeletemethod'
    or lower-case(@MethodName)='skipdatabaselog'
    or lower-case(@MethodName)='skipevents'
    or lower-case(@MethodName)='skipdatamethods'
    or lower-case(@MethodName)='recordlevelsecurity' ]
  where count($calls/*) = 1
  order by $a/@Package
  return
    <Diagnostic>
       <Moniker>SelectUsingFirstOnly</Moniker>
       <Severity>Warning</Severity>
       <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
       <Message>Methods on cursor are called with no parameters, but result is never used.</Message>
       <DiagnosticType>AppChecker</DiagnosticType>
       <Line>{string($calls/@StartLine)}</Line>
       <Column>{string($calls/@StartCol)}</Column>
       <EndLine>{string($calls/@EndLine)}</EndLine>
       <EndColumn>{string($calls/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>
