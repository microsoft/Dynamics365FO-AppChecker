let $q := <R>
{
    for $f in /Form
    for $m in $f//Method[lower-case(@Name)='__initializeextdesign']
    for $se in $m//QualifiedCall/StringLiteralExpression[starts-with(@Value, "'@")]
    return <Res Artifact='{$f/@Artifact}' Method='{$m/@Name}' Value='{$se/@Value}' Caller='{$se/../@MethodName}'>
    {
    }
    </Res>
    
}
</R>

return distinct-values($q/@Caller)
