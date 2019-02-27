(: Check for unbalanced ttsbegin and ttsCommit/ttsabort statements. :)
(: The best practice is that each ttsBegin statement should have a matching ttsCommit statement in the same method and scope. :) 
(: @Language Xpp :)
(: @Author bertd@microsoft.com :)
 
<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/BalancedTtsStatement' Version='1.0'>
{
let $scopes := ("IfStatement", "IfThenElseStatement", "WhileStatement", "DoWhileStatement", "ForStatement", "TryStatement", "CatchStatement", "FinallyStatement", "SearchStatement", "SwitchEntryStatement", "Method", "FunctionDeclaration")
for $a in /(Table | Class | Form | Query)
for $m in $a//Method
for $s in ($m, $m//*[local-name() = $scopes])
where count( ($s/CompoundStatement/TtsBeginStatement) | ($s/TtsBeginStatement) ) != count( ($s/CompoundStatement/(TtsAbortStatement | TtsEndStatement)) | ($s/(TtsAbortStatement | TtsEndStatement)) )
return
    <Diagnostic>
      <Moniker>BalancedTtsStatement</Moniker>
      <Severity>Error</Severity>
      <Path>{string($a/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>ttsBegin statement should have a matching ttsCommit statement in the same method and scope.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($s/@StartLine)}</Line>
      <Column>{string($s/@StartCol)}</Column>
      <EndLine>{string($s/@EndLine)}</EndLine>
      <EndColumn>{string($s/@EndCol)}</EndColumn>
    </Diagnostic>  
}
</Diagnostics>