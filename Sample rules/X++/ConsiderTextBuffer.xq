(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds all statements in the form a += expr where the type of the expression
 is string, when inside a loop. These may (or may not) be eligible to be
 changed to using stringbuilder or TextBuffer, depending on the number of
 expected iterations :)
(: @Category Performance :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Artifacts>
{
    for $c in /*
    for $m in $c//Method 
    for $exprs in $m//(WhileStatement | ForStatement | SearchStatement)//AssignIncrementStatement/*[2][@Type='str']
    order by $m/Name ascending 
    return <ConsiderUsingTextBuffer Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$exprs/@StartLine}' StartCol='{$exprs/@StartCol}'
                EndLine='{$exprs/@EndLine}' EndCol='{$exprs/@EndCol}' />}
</Artifacts>