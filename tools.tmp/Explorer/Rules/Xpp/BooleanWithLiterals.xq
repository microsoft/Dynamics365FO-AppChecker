(: Identify methods that contain true or false literals in an And or Or expression :)
<Classes>
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
</Classes>