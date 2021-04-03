<CaseProblems>
{
    for $a in /Class[@Package='ApplicationSuite']
    for $m in $a//Method
    for $switch in $m//SwitchStatement   
    for $decl in $switch/LocalDeclarationsStatement
    let $declaredVar := $decl/VariableDeclaration/@Name
    
    return <Declaration DeclaredVar='{$declaredVar}' Artifact='{$a/@Artifact}' Package='{$a/@Package}'
                StartLine='{$decl/@StartLine}' StartCol='{$decl/@StartCol}'
                EndLine='{$decl/@EndLine}' EndCol='{$decl/@EndCol}'> 
    {
        for $nextCaseOrDefault in ($decl/following-sibling::CaseValues, $decl/following-sibling::CaseDefault)[1]

        for $fieldRef in $nextCaseOrDefault/following-sibling::*//SimpleField[@Name=$declaredVar]
        return <Reference Artifact='{$a/@Artifact}' Package='{$a/@Package}'
            StartLine='{$fieldRef/@StartLine}' StartCol='{$fieldRef/@StartCol}'
            EndLine='{$fieldRef/@EndLine}' EndCol='{$fieldRef/@EndCol}' >
            {
            }
        </Reference>
   } </Declaration>
}
</CaseProblems>
