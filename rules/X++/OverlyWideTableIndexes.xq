(: Find tables with indexes that have 50+ columns. :)
(: This is not supported in Dynamics 365 for Finance and Operations. :)
(: @Language Xpp :)
(: @Author Andreas.Nielsen@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/OverlyWideTableIndexes' Version='1.0'>
{
for $t in /Table
for $i in $t/Metadata/Indexes/AxTableIndex
where count($i/Fields/AxTableIndexField) >= 50
return
  <Diagnostic>
    <Moniker>OverlyWideTableIndexes</Moniker>
    <Severity>Error</Severity>
    <Path>{string($t/@PathPrefix)}</Path>
    <Message>Indexes with 50+ columns are not allowed.</Message>
    <DiagnosticType>AppChecker</DiagnosticType>
    <Line>{string($t/@StartLine)}</Line>
    <Column>{string($t/@StartCol)}</Column>
    <EndLine>{string($t/@EndLine)}</EndLine>
    <EndColumn>{string($t/@EndCol)}</EndColumn>
  </Diagnostic>
}
</Diagnostics>
