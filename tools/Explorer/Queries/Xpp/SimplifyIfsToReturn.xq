(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find if statements that have the form if (e) return true else return false :)
(: These can be simplified to just returning the expression as is or negated  :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<SimplifyIfWithReturns>
{
 	
    for $c in /*
    for $m in $c//Method
    for $f in $m//IfThenElseStatement
    let $thenPart := $f/*[2]
    let $elsePart := $f/*[3]
    
    where 
    	( 
    	    (name($thenPart) = "CompoundStatement" and count($thenPart/*) = 1 and $thenPart/ReturnStatement/BooleanLiteralExpression)
      	 or (name($thenPart) = 'ReturnStatement' and $thenPart/BooleanLiteralExpression)
      	)
      	and
    	( 
            (name($elsePart) = "CompoundStatement" and count($elsePart/*) = 1 and $elsePart/ReturnStatement/BooleanLiteralExpression)
      	 or (name($elsePart) = 'ReturnStatement' and $elsePart/BooleanLiteralExpression)
      	)

    	
    return <Res Artifact='{$c/@Artifact}' Method='{$m/@Name}'
                StartLine='{$f/@StartLine}' StartCol='{$f/@StartCol}'
                EndLine='{$f/@EndLine}' EndCol='{$f/@EndCol}' />}
</SimplifyIfWithReturns>