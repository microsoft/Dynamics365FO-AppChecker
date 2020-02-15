(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Identify try / catch / finally statements with empty finally blocks :)
(: @Category BestPractice :)
(: @Language Xpp :)

declare function local:SourceLines($s as xs:string, $fromLine as xs:integer, $toLine as xs:integer) as xs:string
{

    let $lines := tokenize($s, '(\r\n?|\n\r?)') 
    return string-join($lines[position() >= max((1, $fromLine - 3)) 
                          and position() <= min((count($lines), $toLine + 3)) ], '&#10;' )
};

<TryWithoutCatchWithFinally>
{
    (: If there is an odd number of children under the try statement, then
       the last one is the finally part: There is 1 for the try block, then
       pairs for (catch, handler), then possibly the finally block :)
   
    for $c in /(Class | Table | Form | Query)
    for $m in $c//Method
    for $t in $m//TryStatement
    where count($t/*) = 2 (: One for try block and one for finally :)
    order by $c/@Package
    return <Res Artifact='{$c/@Artifact}' Package='{$c/@Package}' Method='{$m/@Name}'
                StartLine='{$t/@StartLine}' StartCol='{$t/@StartCol}'
                EndLine='{$t/@EndLine}' EndCol='{$t/@EndCol}'>
    {
            concat('&#10;', local:SourceLines($c/@Source, $t/@StartLine, $t/@EndLine), '&#10;')
    }
    </Res>

}
</TryWithoutCatchWithFinally>