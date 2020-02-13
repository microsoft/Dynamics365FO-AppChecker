(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Order artifacts in order of lines. Limit to results with more than 1000 lines :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
  for $a in /(Class | Table )
  let $lines := $a/@EndLine - $a/@StartLine + 1
  where $lines > 1000
  order by $lines descending
  return <Result Artifact='{$a/@Artifact}' Lines='{$lines}'/>
}
</Results>
