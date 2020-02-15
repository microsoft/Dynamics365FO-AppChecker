(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Calculate the complexity of methods :) 
(: @Category BestPractice :)
(: @Language Xpp :)

(:
  Simple rule of thumb: Count the branching and looping constructs and add 1. The if statements, for, 
  while, and do/while constructs, each count as one. For the switch/case constructs, each case block counts as one. 
  In if and ladder if constructs, the final else does not count. For switch/case constructs, the default block does not count.
:)
declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
      1 + count($m//IfStatement) +  count($m//IfThenElseStatement) +  count($m//WhileStatement) + + count($m//DoWhileStatement)
      + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

<Complexities Category='BestPractice' href='docs.microsoft.com/Socratex/Complexity' Version='1.0'>
{
  (: Only list methods more complex than this: :)
  let $limit := 30
  for $c in /Class | /Table | /Form | /Query
  for $m in $c//Method
  let $cmpl := local:MethodComplexity($m)
  order by $cmpl descending
  where $cmpl > $limit
  return <Complexity Artifact='{$c/@Artifact}' Method='{$c/@Name || '.' || $m/@Name}' ComplexityNumber='{$cmpl}' 
     StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' 
     EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />
}
</Complexities>