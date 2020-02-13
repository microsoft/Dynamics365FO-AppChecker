(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Classes where not all Public or protected methods are documented. :)
(: The quality of the documentation is not checked in this query.    :)
(: Only public and protected methods are considered here. The query is 
   easily modified to take into account all methods, including private ones:)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<ClassesWithUndocumentedMethods Category='BestPractice' href='docs.microsoft.com/Socratex/ClassesWithUndocumentedMethods' Version='1.0'>
{
  for $c in /Class | /Table | /Form
  let $methodCount := count($c//Method[@IsPublic='True' or @IsProtected='True'])
  let $commentedMethodCount := count($c//Method[(@IsPublic='True' or @IsProtected='True') and @Comments != ""])
  let $documentationFactor := $commentedMethodCount div $methodCount
  where $methodCount != $commentedMethodCount
  order by $documentationFactor
  return 
      <CommentingFactor Artifact='{$c/@Artifact}' Methods='{$methodCount}' CommentedMethods='{$commentedMethodCount}' Percent='{100*$documentationFactor}'
            StartLine='{$c/@StartLine}' StartCol='{$c/@StartCol}' EndLine='{$c/@EndLine}' EndCol='{$c/@EndCol}'>
      {
        for $m in $c//Method[(@IsPublic='True' or @IsProtected='True')  and @Comments = ""]
            return <Method Artifact='{$c/@Artifact}' Name='{$m/@Name}' 
                           StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' 
                           EndLine='{$m/@EndLine}' EndCol='{$c/@EndCol}'/>
      }
      </CommentingFactor>
}
</ClassesWithUndocumentedMethods>