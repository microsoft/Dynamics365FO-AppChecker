(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify methods that contain true or false literals in an And or Or expression :)
(: @Language Xpp :)
(: @Category Mandatory :)

<BooleanWithLiterals Category='Mandatory' href='docs.microsoft.com/Socratex/BooleanWithLiterals' Version='1.0'>
{
    for $c in /*
    for $m in $c//Method 
    let $exprs := $m//OrExpression/BooleanLiteralExpression
                | $m//AndExpression/BooleanLiteralExpression
    where $exprs
    order by $m/Name ascending 
    return <Res Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$exprs/@StartLine}' StartCol='{$exprs/@StartCol}'
                EndLine='{$exprs/@EndLine}' EndCol='{$exprs/@EndCol}' />}
</BooleanWithLiterals>