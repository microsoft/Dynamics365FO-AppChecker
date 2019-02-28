(: Finds references to objects from namespace System.Reflection :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/SystemReflection' Version='1.0'>
{
  for $a in //(Class | Table | Form | Query)
  for $m in $a//Method
  where $m//*[starts-with(@Type, "System.Reflection")]
  return
    <Diagnostic>
      <Moniker>SystemReflection</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>This method uses System.Reflection. This can cause maintainability issues as referenced code may change.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($m/@StartLine)}</Line>
      <Column>{string($m/@StartCol)}</Column>
      <EndLine>{string($m/@EndLine)}</EndLine>
      <EndColumn>{string($m/@EndCol)}</EndColumn>
    </Diagnostic>  
} 
</Diagnostics>
