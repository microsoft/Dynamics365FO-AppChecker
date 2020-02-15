(: Non-private methods on non-private artifacts that do not have any documentation :)
(: @Language Xpp :)

declare namespace functx = "http://www.functx.com";
declare function functx:trim ( $arg as xs:string? )  as xs:string
{
  replace(replace($arg,'\s+$',''),'^\s+','')
};

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/MissingMethodDocumentation' Version='1.0'>
{
  for $c in /(Class | Table | Form)[@IsPrivate = "false"]
  where $c//Method[@IsPrivate = "false" and functx:trim(@Comments) = ""]
  return
    <Diagnostic>
      <Moniker>MissingMethodDocumentation</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($c/@PathPrefix)}</Path>
      <Message>Documentation is missing for non-private method on this non-private class. XML documentation should be created to provide information related to usage.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($c/@StartLine)}</Line>
      <Column>{string($c/@StartCol)}</Column>
      <EndLine>{string($c/@EndLine)}</EndLine>
      <EndColumn>{string($c/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>