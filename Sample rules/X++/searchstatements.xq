<T>
{
   for $c in /Class[@Package='ApplicationFoundation']
   for $m in $c/Method
   for $ss in $m//SearchStatement
   return <SearchStatment Artifact='{$c/@Artifact}' Method='{$m/@Name}'
           StartLine='{$ss/@StartLine}' StartCol='{$ss/@StartCol}' EndLine='{$ss/@EndLine}' EndCol='{$ss/@EndCol}'/>
  
}
</T>
