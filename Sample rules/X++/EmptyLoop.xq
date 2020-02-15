(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Show the occurrence of empty statements :)
(: @Category BestPractice :)
(: @Language Xpp :)

<EmptyStatements>
{
for $c in /Class
for $m in $c/Method
for $e in $m//EmptyStatement
   return <EmptyStatement Artifact='{$c/@Artifact}' Method='{$m/@Name}' 
      StartLine='{$e/@StartLine}' StartCol='{$e/@StartCol}'
      EndLine='{$e/@EndLine}' EndCol='{$e/@EndCol}'/>
}
</EmptyStatements>