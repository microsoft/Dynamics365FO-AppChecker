(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find all empty compounds statements. This will not find empty methods. :)
(: @Language Xpp :)
(: @Category Informational :)
(: @Author pvillads@microsoft.com :)

<EmptyCompounds Category='Informational' href='docs.microsoft.com/Socratex/EmptyCompounds' Version='1.0'>
{
    for $c in /*
    for $compounds in $c//CompoundStatement
    let $cnt := count($compounds/*)
    where $cnt = 0
    return <EmptyCompound Artifact='{$c/@Artifact}' Method='{$c/@Name}' 
          StartLine='{$compounds/@StartLine}' StartCol='{$compounds/@StartCol}'
          EndLine='{$compounds/@EndLine}' EndCol='{$compounds/@EndCol}' />
}
</EmptyCompounds>