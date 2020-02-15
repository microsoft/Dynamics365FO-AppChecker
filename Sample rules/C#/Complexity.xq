(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Calculate the complexity of methods :)
(: @Category BestPractice :)
(: @Language C# :)

(:
  Simple rule of thumb: Count the branching and looping constructs and add 1. The if statements, for,
  while, and do/while constructs, each count as one. For the switch/case constructs, each case block counts as one.
  In if and ladder if constructs, the final else does not count. For switch/case constructs, the default block does not count.
:)
declare function local:MethodComplexity($m as element(MethodDeclaration)) as xs:integer
{
      1 + count($m//IfStatement) +  count($m//WhileStatement) + + count($m//DoStatement)
      + count($m//ForStatement) + count($m//ForEachStatement) + count($m//SwitchSection/*) + count($m//ConditionalExpression)
};

<Complexities Category='BestPractice' href='docs.microsoft.com/Socratex/Complexity' Version='1.0'>
{
  (: Only list methods more complex than this: :)
  let $limit := 5
  for $c in //ClassDeclaration
  for $m in $c//MethodDeclaration
  let $cmpl := local:MethodComplexity($m)
  order by $cmpl descending
  where $cmpl > $limit
  return <Complexity Artifact='{$c/@Artifact}' Language="C#" Method='{$c/@Name || '.' || $m/@Name}' ComplexityNumber='{$cmpl}'
     StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}'
     EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />

}
</Complexities>