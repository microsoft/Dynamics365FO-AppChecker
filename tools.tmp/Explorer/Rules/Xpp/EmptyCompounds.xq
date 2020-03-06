(: Find all empty compounds statements. This will not find empty methods. :)
<Results>
{
    for $c in /*
    for $compounds in $c//CompoundStatement
    let $cnt := count($compounds/*)
    where $cnt = 0
    return <Result Artifact='{$c/@Artifact}' Method='{$c/@Name}' 
          StartLine='{$compounds/@StartLine}' StartCol='{$compounds/@StartCol}'
          EndLine='{$compounds/@EndLine}' EndCol='{$compounds/@EndCol}' />
}
</Results>