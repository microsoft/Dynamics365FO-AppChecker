(: Finds methods with missing Update/doUpdate after ForUpdate statement :)
(: @Language Xpp :)

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/SelectForUpdateAbsent' Version='1.0'>
{
  for $a in /*
  for $m in $a/Method
  for $q in $m//Query
  where $q/data(SelectionHints) = "forupdate"
  let $obj := lower-case($q/data(@BufferName))
  where not(fn:exists($m//QualifiedCall[lower-case(@MethodName) = ("doupdate", "update", "delete", "dodelete", "write")]/SimpleQualifier[lower-case(@Name) = $obj]))
  return
    <Diagnostic>
      <Moniker>SelectForUpdateAbsent</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method contains a "select forupdate" statement, but does not perform the corresponding write operation. Review the code to make sure a call to an update, delete, or write method is not missing.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($q/@StartLine)}</Line>
      <Column>{string($q/@StartCol)}</Column>
      <EndLine>{string($q/@EndLine)}</EndLine>
      <EndColumn>{string($q/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>
