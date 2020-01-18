declare function local:visitStatement($s as element()) as xs:integer
{
    if (local-name($s) = "CompoundStatement") then
       local:visitCompoundStatement($s)  
    else if (local-name($s) = ("WhileStatement", "IfStatement",  
                               "DoWhileStatement", "ForStatement", "SearchStatement",
                               "SwitchStatement", "ConditionalExpression" )) then
       local:visitIndentedStatement($s)  
 	else if (local-name($s) = ("IfThenElseStatement")) then
 	   local:visitIfThenElseStatement($s)
    else 0
};

(: This is special because we do not want to have if () then ... else if ... 
   to be seen as nested :)
declare function local:visitIfThenElseStatement($s as element()) as xs:integer
{
    let $condlevel := local:visitStatement($s/*[1])
    let $thenlevel := local:visitStatement($s/*[2])
    
    let $elselevel := if (local-name($s/*[3]) = 'IfThenElseStatement') then
    	local:visitIfThenElseStatement($s/*[3])
    else
    	local:visitStatement($s/*[3])

    return fn:max(($condlevel, $thenlevel, $elselevel)) 
};

declare function local:visitIndentedStatement($s as element()) as xs:integer
{
  1 + max($s/*/local:visitStatement(.))
};

declare function local:visitCompoundStatement($s as element(CompoundStatement)) as xs:integer
{
  if ($s/*) then fn:max($s/*/local:visitStatement(.)) else 0
};

declare function local:visitMethod($m as element(Method)) as xs:integer
{
  if ($m/*) then max($m/*/local:visitStatement(.)) else 0
};

<StatementIndentationLevels>
{
  let $threshold := 4
  
  for $c in /Class
  for $m in $c/Method
     let $level := local:visitMethod($m)
     where $level > $threshold
     order by $level descending
     return <Level Artifact='{$c/@Artifact}'  Method='{$m/@Name}' Level='{$level}'
                   StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' 
                   EndLine='{$m/@EndLine}' EndCol='{$c/@EndCol}' />
}
</StatementIndentationLevels>

