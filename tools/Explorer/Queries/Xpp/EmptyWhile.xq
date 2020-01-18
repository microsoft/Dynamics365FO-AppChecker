(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Catch while loops with empty bodies. :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  order by $a/@Artifact
  
  let $emptyCompound := for $emptyWhile in $m//WhileStatement/CompoundStatement[count(./*) = 0]
                        return <Result Artifact='{$a/@Artifact}'
                             StartLine='{$emptyWhile/@StartLine}' StartCol='{$emptyWhile/@StartCol}' EndLine='{$emptyWhile/@EndLine}' EndCol='{$emptyWhile/@EndCol}'/>
                             
  let $emptyStmt :=     for $emptyWhile in $m//WhileStatement/EmptyStatement
                        return <Result Artifact='{$a/@Artifact}'
                             StartLine='{$emptyWhile/@StartLine}' StartCol='{$emptyWhile/@StartCol}' EndLine='{$emptyWhile/@EndLine}' EndCol='{$emptyWhile/@EndCol}'/>
                             
  return ($emptyCompound, $emptyStmt)                           
                                                      
}
</Results>
