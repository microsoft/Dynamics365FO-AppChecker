(: Get artifacts that are extention classes but don't have a prefix :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/ExtensionsWithoutPrefix' Version='1.0'>
{
  for $a in /Class
  where $a/@ExtensionOfAttributeExists = "true" 
  let $targetClass := $a/@ExtensionTarget
  where $a/@Name = fn:concat($targetClass, "_Extension")
  let $typeNamePair := fn:tokenize($a/@Artifact, ":")  
  return 
    <Diagnostic>
      <Moniker>ExtensionsWithoutPrefix</Moniker>
      <Severity>Error</Severity>
      <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}</Path>
      <Message>Extension class name should include a prefix</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
    </Diagnostic>  
}
</Diagnostics>
