(: Check usage of AOSAuthorization property for a table in chain of inheritance :)
(: Verify that both table have either authorization enabled or disabled :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)
 
<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/TableTPFIntegrity' Version='1.0'>
{
  for $child in /Table[@SupportInheritance = "true" and @Extends != ""]
  where $child//AosAuthorization
  let $childAuth := $child//data(AosAuthorization)
  let $parentTable := $child/string(@Extends)
  let $parentAuth := /Table[@Name = $parentTable]//data(AosAuthorization)
  where $parentAuth != ""
  and ( ($parentAuth = "CreateDelete" and $childAuth != "CreateDelete" and $childAuth != "CreateReadUpdateDelete" and $childAuth != "CreateUpdateDelete")
  or ($parentAuth = "CreateReadUpdateDelete" and $childAuth != "CreateReadUpdateDelete")
  or ($parentAuth = "CreateUpdateDelete" and $childAuth != "CreateReadUpdateDelete" and $childAuth != "CreateUpdateDelete")
  or ($parentAuth = "Read" and $childAuth != "Read" and $childAuth != "CreateReadUpdateDelete")
  or ($parentAuth = "UpdateDelete" and $childAuth != "CreateReadUpdateDelete" and $childAuth != "CreateUpdateDelete" and $childAuth != "UpdateDelete") )
  return
    <Diagnostic>
      <Moniker>TableTPFIntegrity</Moniker>
      <Severity>Error</Severity>
      <Path>{string($child/@PathPrefix)}</Path>
      <Message>Invalid usage of AOSAuthorization property for a table in chain of inheritance. Please ensure that both tables have either AOS authorization enabled or disabled.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($child/@StartLine)}</Line>
      <Column>{string($child/@StartCol)}</Column>
      <EndLine>{string($child/@EndLine)}</EndLine>
      <EndColumn>{string($child/@EndCol)}</EndColumn>
    </Diagnostic>  
} 
</Diagnostics>