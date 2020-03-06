(: List of classes that extend SysAttribute :)

<Results>
{
  let $baseClass := 'SysAttribute'

  for $c in /Class[@Extends=$baseClass]
  return <Result Artifact='{$c/@Artifact}' 
                 StartLine='{$c/@StartLine}' EndLine='{$c/@EndLine}' 
                 StartCol='{$c/@StartCol}' EndCol='{$c/@EndCol}' />
}
</Results>