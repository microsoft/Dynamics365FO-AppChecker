(: methods with parameters not starting with underscores :)
<Results>
{
    for $c in /*
    for $m in $c//Method
    for $p in $m/ParameterDeclaration
    where not(starts-with($p/@Name, '_'))
    return <Result Artifact='{$c/@Artifact}' Method='{$m/@Name}' 
                StartLine='{$p/@StartLine}' StartCol='{$p/@StartCol}'
                EndLine='{$p/@EndLine}' EndCol='{$p/@EndCol}'>
                   {$p/@Name} 
           </Result>  
}
</Results>