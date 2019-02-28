(: Non-private artifacts that do not have any documentation :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

declare namespace functx = "http://www.functx.com";
declare function functx:trim ( $arg as xs:string? )  as xs:string 
{
  replace(replace($arg,'\s+$',''),'^\s+','')
};
 
<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/MissingClassDocumentation' Version='1.0'>
{
  for $c in //(Class | Table | Form | Query)[@IsPrivate = "false" and functx:trim(@Comments) = ""]
  return
    <Diagnostic>
      <Moniker>MissingClassDocumentation</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($c/@PathPrefix)}</Path>
      <Message>Documentation is missing for this non-private class. XML documentation should be created to provide information related to usage.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($c/@StartLine)}</Line>
      <Column>{string($c/@StartCol)}</Column>
      <EndLine>{string($c/@EndLine)}</EndLine>
      <EndColumn>{string($c/@EndCol)}</EndColumn>
    </Diagnostic>  
} 
</Diagnostics>