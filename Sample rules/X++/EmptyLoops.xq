(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find loops with no meaningful bodies :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<EmptyLoops Category='BestPractice' href='docs.microsoft.com/Socratex/EmptyLoops' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $loop in $m/(WhileStatement | ForStatement | DoWhileStatement)
  where $loop/CompoundStatement and count($loop/CompoundStatement/*) = 0
  order by $a/@Name
  return <EmptyLoop Artifact='{$a/@Artifact}'
    StartLine='{$loop/@StartLine}' StartCol='{$loop/@StartCol}' EndLine='{$loop/@EndLine}' EndCol='{$loop/@EndCol}'/>
}
</EmptyLoops>
