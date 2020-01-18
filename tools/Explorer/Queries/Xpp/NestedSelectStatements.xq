(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find places where select statements are nested. :)
(: These cases may or may not be good candidates for joins :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<NestedSelects>
{
    for $c in /*
    for $m in $c/Method
    for $nested in $m//SearchStatement//SearchStatement
    where $nested
    return <NestedSelect Artifact='{$c/@Artifact}' Method='{$m/@Name}'
        StartLine='{$nested/@StartLine}' EndLine='{$nested/@EndLine}'
        StartCol='{$nested/@StartCol}' EndCol='{$nested/@EndCol}' />
}
</NestedSelects>