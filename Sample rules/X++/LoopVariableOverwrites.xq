(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds assignments of variables used in for statements :)
(: If the intention is to break out of the loop, use the break statement. :)
(: @Category BestPractice :)
(: @Language Xpp :)

<LoopVariableOverwrite>
{
    for $a in /*
    for $m in $a//Method
    for $for in $m//ForStatement
    let $n1 := $for/ForFieldAssign/SimpleField/@Name
    
    let $v1 := for $assign in $for//(AssignEqualStatement | AssignIncrementStatement | AssignDecrementStatement)/SimpleField[@Name=$n1]
        return <ForAssign Artifact='{$a/@Artifact}' Method='{$m/@Name}'
            StartLine='{$assign/@StartLine}' StartCol='{$assign/@StartCol}'
            EndLine='{$assign/@EndLine}' EndCol='{$assign/@EndCol}' />
            
    let $v2 := for $nestedfor in $for//ForStatement/ForFieldAssign/SimpleField[@Name=$n1]
        return <NestedFor Artifact='{$a/@Artifact}' Method='{$m/@Name}'
            StartLine='{$nestedfor/@StartLine}' StartCol='{$nestedfor/@StartCol}'
            EndLine='{$nestedfor/@EndLine}' EndCol='{$nestedfor/@EndCol}' />
            
    return ($v1, $v2)
}
</LoopVariableOverwrite>
