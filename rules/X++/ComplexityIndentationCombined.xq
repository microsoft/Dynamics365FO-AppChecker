(: Combines Complexities and MethodIdentationLevel queries :)
(: to Find methods on class or table types with a complexity level above 30 :)
(: Error if maximum indentation of the method is above 2, warning otherwise :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

(: Simple rule of thumb: Count the branching and looping constructs and add 1. The if statements, for, 
  while, and do/while constructs, each count as one. For the switch/case constructs, each case block counts as one. 
  In if and ladder if constructs, the final else does not count. For switch/case constructs, the default block does not count. :)
declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
      1 + count($m//IfStatement) +  count($m//IfThenElseStatement) +  count($m//WhileStatement) + count($m//DoWhileStatement)
      + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

(: The following functions are used to calculate the maximum indentation of a method :)
declare function local:maxIndentationOfStatement($s as element()) as xs:integer
{
    if (local-name($s) = "CompoundStatement") then
       local:visitCompoundStatement($s)  
    else if (local-name($s) = ("WhileStatement", "IfStatement",  
                               "DoWhileStatement", "ForStatement", "SearchStatement",
                               "SwitchStatement", "ConditionalExpression" )) then
       local:maxIndentationOfIndentedStatement($s)  
     else if (local-name($s) = ("IfThenElseStatement")) then
        local:maxIndentationOfIfThenElseStatement($s)
    else 0
};

(: This is special because we do not want to have if () then ... else if ... 
   to be seen as nested :)
declare function local:maxIndentationOfIfThenElseStatement($s as element()) as xs:integer
{
    let $condlevel := local:maxIndentationOfStatement($s/*[1])
    let $thenlevel := local:maxIndentationOfStatement($s/*[2])
    
    let $elselevel := if (local-name($s/*[3]) = 'IfThenElseStatement') then
        local:maxIndentationOfIfThenElseStatement($s/*[3])
    else
        local:maxIndentationOfStatement($s/*[3])

    return fn:max(($condlevel, $thenlevel, $elselevel)) 
};

declare function local:maxIndentationOfIndentedStatement($s as element()) as xs:integer
{
  1 + max($s/*/local:maxIndentationOfStatement(.))
};

declare function local:visitCompoundStatement($s as element(CompoundStatement)) as xs:integer
{
  if ($s/*) then fn:max($s/*/local:maxIndentationOfStatement(.)) else 0
};

declare function local:visitMethod($m as element(Method)) as xs:integer
{
  if ($m/*) then max($m/*/local:maxIndentationOfStatement(.)) else 0
};

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/ComplexityIndentation' Version='1.0'>
{
  (: Define thresholds for complexity and indentation: :)
  let $complexityLimit := 30
  let $indentationLimit := 2
  
  for $c in /(Class | Table | Form | Query)
  for $m in $c//Method
  let $cmpl := local:MethodComplexity($m)
  where $cmpl > $complexityLimit  
  order by $cmpl descending
  let $level := local:visitMethod($m)
  let $typeNamePair := fn:tokenize($c/@Artifact, ":")
  return
    <Diagnostic>
      <Moniker>ComplexityIndentationCombined</Moniker>
      <Severity>{if ($level > $indentationLimit) then "Error" else "Warning"}</Severity>
      <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}/Method/{string($m/@Name)}</Path>
      <Message>Method complexity is above {$complexityLimit}{if ($level > $indentationLimit) then concat(", indentation is more than ", $indentationLimit) else ""}</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($m/@StartLine)}</Line>
      <Column>{string($m/@StartCol)}</Column>
      <EndLine>{string($m/@EndLine)}</EndLine>
      <EndColumn>{string($m/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>