(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Returns ifstatements that have empty constituents. :)
(: This is different from empty compound statements, because the offending
   statement can be have either empty compound statements, or empty statement
   as its children :)
(: @Category Mandatory :)
(: @Language Xpp :)

declare function local:SourceLines($s as xs:string, $fromLine as xs:integer, $toLine as xs:integer) as xs:string
{

    let $lines := tokenize($s, '(\r\n?|\n\r?)') 
    return string-join($lines[position() >= max((1, $fromLine - 3)) 
                          and position() <= min((count($lines), $toLine + 3)) ], '&#10;' )
};

<EmptyIfParts Category='Mandatory' href='docs.microsoft.com/Socratex/EmptyIfParts' Version='1.0'>
{
    for $c in /*
    for $m in $c//Method
    for $e in $m//(IfStatement | IfThenElseStatement)/(CompoundStatement | EmptyStatement)
    where not($e/*)
    return <EmptyIfPart Artifact='{$c/@Artifact}' Method='{$m/@Name}' 
      StartLine='{$e/@StartLine}' StartCol='{$e/@StartCol}'
      EndLine='{$e/@EndLine}' EndCol='{$e/@EndCol}'>
      {
        concat('&#10;', local:SourceLines($c/@Source, $e/@StartLine, $e/@EndLine), '&#10;')
      }
   </EmptyIfPart>
}
</EmptyIfParts>