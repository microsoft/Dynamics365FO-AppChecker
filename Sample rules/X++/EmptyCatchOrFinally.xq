(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify try / catch / finally statements with empty blocks :)
(: @Language Xpp :)
(: @Category BestPractice :)

<EmptyCatchOrFinallys Category='BestPractice' href='docs.microsoft.com/Socratex/EmptyCatchOrFinallys' Version='1.0'>
{
    (: If there is an odd number of children under the try statement, then
       the last one is the finally part: There is 1 for the try block, then
       pairs for (catch, handler), then possibly the finally block :)
   
    for $c in /*
    for $m in $c//Method
    for $f in $m//TryStatement/(EmptyStatement | CompoundStatement[not(*)])
    
    return <EmptyCatchOrFinally Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$f/@StartLine}' StartCol='{$f/@StartCol}'
                EndLine='{$f/@EndLine}' EndCol='{$f/@EndCol}' />}
</EmptyCatchOrFinallys>