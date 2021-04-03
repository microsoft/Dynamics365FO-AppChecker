(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find switch statements with case entries that declare items that 
are reused in other cases. This is dangerous since there is a risk
that the item is not declared, depending on the switch value.
This is semantically correct in X++ but makes maintenance much more difficult :)

(: Note: This does not find references updated with byref parameters :)
(: @Language Xpp :)
(: @Category Informational :)

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
