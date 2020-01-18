(: Find places where select statements are nested. :)
(: These cases may or may not be good candidates for joins :)
<Results>
{
    for $c in /*
    for $m in $c/Method
    for $nested in $m//SearchStatement//SearchStatement
    where $nested
    return <Result Artifact='{$c/@Artifact}' Method='{$m/@Name}'
        StartLine='{$nested/@StartLine}' EndLine='{$nested/@EndLine}'
        StartCol='{$nested/@StartCol}' EndCol='{$nested/@EndCol}' />
}
</Results>