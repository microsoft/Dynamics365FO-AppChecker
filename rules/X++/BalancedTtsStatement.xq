(: Check for unbalanced ttsbegin and ttsCommit/ttsabort statements. :)
(: The best practice is that each ttsBegin statement should have a matching ttsCommit statement in the same method and scope. :)
(: @Language Xpp :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/BalancedTtsStatement' Version='1.0'>
{
let $scopes := ("IfStatement", "IfThenElseStatement", "WhileStatement", "DoWhileStatement", "ForStatement", "TryStatement", "CatchStatement", "FinallyStatement", "SearchStatement", "SwitchEntryStatement", "Method", "FunctionDeclaration")
for $a in /*
for $m in $a//Method
for $s in ($m, $m//*[local-name() = $scopes])
let $ttsbegin := count( ($s/CompoundStatement/TtsBeginStatement) | ($s/TtsBeginStatement) )
let $ttsend := count( ($s/CompoundStatement/(TtsAbortStatement | TtsEndStatement)) | ($s/(TtsAbortStatement | TtsEndStatement)) )
where $ttsbegin != $ttsend
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