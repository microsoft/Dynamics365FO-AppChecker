(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify methods that contain calls to strlen with a string concatenation. 
   These are eligible to writing to adding the lengths of the addition operands :)
(: @Category Performance :)
(: @Language Xpp :)

<NeedlessStringConcatenations>
{
    for $c in /*
    for $m in $c//Method 
    for $exprs in $m//FunctionCall[@FunctionName='strlen']/AddExpression
    order by $m/Name ascending 
    return <Result Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$exprs/@StartLine}' StartCol='{$exprs/@StartCol}'
                EndLine='{$exprs/@EndLine}' EndCol='{$exprs/@EndCol}' />}
</NeedlessStringConcatenations>