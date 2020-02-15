(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Calculate the distribution of the lines of code in methods. :)
(: @Category Informational :)
(: @Language Xpp :)

(: Calculate the sequence containing all the line counts :)
let $seq := 
  for $a in /(Class | Table | Query | Form)
  for $m in $a//Method
  let $l := ($m/@EndLine - $m/@StartLine + 1)
  order by $l
  return $l

return <MethodLineCounts Mean='{sum($seq) div count($seq)}'>
{
  for $dv in distinct-values($seq) 
  return <MethodLineCount val = '{$dv}' Count = '{count(for $i in $seq where $i = $dv return 1)}'/>
}
</MethodLineCounts>
