(: Get queries not using firstonly, nofetch, or fields explicit selection  :)
(: @Language Xpp :)

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/SelectUsingFirstOnly' Version='1.0'>
{
  let $hints := ("FirstOnly1", "FirstOnly", "NoFetch")
  for $a in /*
  for $m in $a//Method
  for $q in $m//Query
  where string(node-name($q/..)) != "JoinSpecification" (: only outermost query :)
  and not(fn:exists($q/SelectionHints[. = $hints]))
  and fn:exists($q/AllFieldsSelection)
  return
    <Diagnostic>
      <Moniker>SelectUsingFirstOnly</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>Firstonly should be used in queries when a single record is intended to be returned.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($q/@StartLine)}</Line>
      <Column>{string($q/@StartCol)}</Column>
      <EndLine>{string($q/@EndLine)}</EndLine>
      <EndColumn>{string($q/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>
