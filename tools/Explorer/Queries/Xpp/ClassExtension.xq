(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: List of classes that extend SysAttribute :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
  let $baseClass := 'SysAttribute'

  for $c in /Class[@Extends=$baseClass]
  return <Result Artifact='{$c/@Artifact}' 
                 StartLine='{$c/@StartLine}' EndLine='{$c/@EndLine}' 
                 StartCol='{$c/@StartCol}' EndCol='{$c/@EndCol}' />
}
</Results>