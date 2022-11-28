(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find references cross compnay delete statements. :)

(: @Language Xpp :)
(: @Category Informational :)
<Results>
{
  for $a in /(Class | Table | Form | Query)
  for $stmt in $a//DeleteStatement//(CrossCompanyAll | CrossCompanyContainer)
  order by $a/@Package
  return <Result Artifact='{$a/@Artifact}' Package='{$a/@Package}'
    StartLine='{$stmt/@StartLine}' StartCol='{$stmt/@StartCol}' EndLine='{$stmt/@EndLine}' EndCol='{$stmt/@EndCol}'/>
}
</Results>
