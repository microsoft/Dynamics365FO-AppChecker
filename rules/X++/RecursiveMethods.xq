(: Identify recursive methods :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Diagnostics Category="Best practice" href="docs.microsoft.com/Socratex/RecursiveMethods" Version="1.0">
{
  for $a in /(Class | Table | Query | Form)
  return (
    for $m in $a//Method[lower-case(@IsStatic)='true']
      for $call in $m//StaticMethodCall
      where $call[lower-case(@ClassName)=lower-case($a/@Name) and lower-case(@MethodName)=lower-case($m/@Name)]
      return
          <Diagnostic>
            <Moniker>RecursiveStaticMethod</Moniker>
            <Severity>Error</Severity>
            <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
            <Message>This static method is recursive. If the recursion is based on business data, there is a risk that the stack will overflow causing irreversible breakdown</Message>
            <DiagnosticType>AppChecker</DiagnosticType>
            <Line>{string($call/@StartLine)}</Line>
            <Column>{string($call/@StartCol)}</Column>
            <EndLine>{string($call/@EndLine)}</EndLine>
            <EndColumn>{string($call/@EndCol)}</EndColumn>
          </Diagnostic>
   ,
    for $m in $a//Method[lower-case(@IsStatic) !='true']
      for $call in $m//QualifiedCall/SimpleQualifier
      where $call[lower-case(@Type)=lower-case($a/@Name) and lower-case($m/@Name) = lower-case($call/../@MethodName)]
      return
          <Diagnostic>
            <Moniker>RecursiveInstanceMethod</Moniker>
            <Severity>Error</Severity>
            <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
            <Message>This instance method is recursive. If the recursion is based on business data, there is a risk that the stack will overflow causing irreversible breakdown</Message>
            <DiagnosticType>AppChecker</DiagnosticType>
            <Line>{string($call/@StartLine)}</Line>
            <Column>{string($call/@StartCol)}</Column>
            <EndLine>{string($call/@EndLine)}</EndLine>
            <EndColumn>{string($call/@EndCol)}</EndColumn>
          </Diagnostic>
  )
}
</Diagnostics>
