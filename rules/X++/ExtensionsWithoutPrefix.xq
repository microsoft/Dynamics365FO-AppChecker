(: Get artifacts that are extention classes but don't have a prefix :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/ExtensionsWithoutPrefix' Version='1.0'>
{
  for $a in //Class
  where $a/AttributeList/Attribute[@Name = "ExtensionOf"] 
  let $targetClass := $a/string(@ExtensionTarget)
  where $a/@Name = fn:concat($targetClass, "_Extension")
  return 
    <Diagnostic>
      <Moniker>ExtensionsWithoutPrefix</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}</Path>
      <Message>Extension class name should include a prefix</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($a/@StartLine)}</Line>
      <Column>{string($a/@StartCol)}</Column>
      <EndLine>{string($a/@EndLine)}</EndLine>
      <EndColumn>{string($a/@EndCol)}</EndColumn>      
    </Diagnostic>  
}
</Diagnostics>
