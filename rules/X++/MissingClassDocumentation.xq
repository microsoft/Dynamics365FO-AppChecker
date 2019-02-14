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
  for $c in (/Class | /Table | /Form)[@IsPrivate = "false" and functx:trim(@Comments) = ""]
  let $typeNamePair := fn:tokenize($c/@Artifact, ":")  
      return
        <Diagnostic>
          <Moniker>MissingClassDocumentation</Moniker>
          <Severity>Warning</Severity>
          <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}</Path>
          <Message>Documentation is missing for this non-private {$typeNamePair[1]}. XML documentation should be created to provide information related to usage.</Message>
          <DiagnosticType>AppChecker</DiagnosticType>
        </Diagnostic>  
} 
</Diagnostics>