(: Finds references To Insert and doInsert in loops :)
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)

<Diagnostics Category='Best practice' href='docs.microsoft.com/Socratex/InsertInLoop' Version='1.0'>
{
  let $ins := ("Insert", "doInsert")
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $s in $m//(SearchStatement | DoWhileStatement | ForStatement | WhileStatement)
  for $e in $s//CompoundStatement/ExpressionStatement//QualifiedCall[@MethodName = $ins]
  let $type := $e/(ExpressionQualifier | SimpleQualifier)/@Type
  where not(exists(for $t in /Table[@Name = $type] where exists($t//TableType) return 1))
  return
    <Diagnostic>
      <Moniker>InsertInLoop</Moniker>
      <Severity>Warning</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>Consider using insert_recordset or a RecordInsertList list to bundle database inserts inside a loop.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($e/@StartLine)}</Line>
      <Column>{string($e/@StartCol)}</Column>
      <EndLine>{string($e/@EndLine)}</EndLine>
      <EndColumn>{string($e/@EndCol)}</EndColumn>
    </Diagnostic>  
}
</Diagnostics>