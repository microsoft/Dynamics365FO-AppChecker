(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify try / catch / finally statements with empty finally blocks :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<EmptyFinallys>
{
    (: If there is an odd number of children under the try statement, then
       the last one is the finally part: There is 1 for the try block, then
       pairs for (catch, handler), then possibly the finally block :)
   
    for $c in /*
    for $m in $c//Method
    for $t in $m//TryStatement
    where count($t/*) mod 2 = 0
    let $f := $t/*[position() = last()]
    let $compoundChild := $f/*[1]
    where not( $compoundChild)
    return <Res Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$f/@StartLine}' StartCol='{$f/@StartCol}'
                EndLine='{$f/@EndLine}' EndCol='{$f/@EndCol}' />}
</EmptyFinallys>