(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Find TODO comments in the code, which indicate unfinished code :)

(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)
<TODOComments>
{
  for $c in /*
    for $comment in $c//*[contains(lower-case(@Comments), "todo")]
      return <Todo Artifact='{$c/@Artifact}' 
              StartLine='{$comment/@StartLine}' EndLine='{$comment/@EndLine}' 
              StartCol='{$comment/@StartCol}' EndCol='{$comment/@EndCol}'>
              {$comment/@Comments}
          </Todo>
}
</TODOComments>