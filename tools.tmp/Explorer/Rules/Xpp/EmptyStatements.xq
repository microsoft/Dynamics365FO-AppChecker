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