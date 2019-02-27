(: Non-private methods on non-private artifacts that do not have any documentation :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

declare namespace functx = "http://www.functx.com";
declare function functx:trim ( $arg as xs:string? )  as xs:string 
{
  replace(replace($arg,'\s+$',''),'^\s+','')
};
 
<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/MissingMethodDocumentation' Version='1.0'>
{
  for $c in /(Class | Table | Form)[@IsPrivate = "false"]
  for $m in $c//Method[@IsPrivate = "false" and functx:trim(@Comments) = ""]
  return
    <Diagnostic>
      <Moniker>MissingMethodDocumentation</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($c/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>Documentation is missing for non-private method on non-private class. XML documentation should be created to provide information related to usage.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($m/@StartLine)}</Line>
      <Column>{string($m/@StartCol)}</Column>
      <EndLine>{string($m/@EndLine)}</EndLine>
      <EndColumn>{string($m/@EndCol)}</EndColumn>
    </Diagnostic>  
} 
</Diagnostics>