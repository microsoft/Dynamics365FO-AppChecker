declare variable $NOCALLSNEXT as xs:string := "NOCALLSNEXT";
declare variable $MAYCALLNEXT as xs:string := "MAYCALLNEXT";
declare variable $CALLSNEXT as xs:string := "CALLSNEXT";

declare variable $NOBREAKSFLOW := "NOBREAKSFLOW";
declare variable $MAYBREAKFLOW := "MAYBREAKFLOW";
declare variable $BREAKSFLOW := "BREAKSFLOW";

(: Analyze a method, returning the Calls next status, i.e. one of 
   the values defined above :)
declare function local:visitMethodContent($method as element(Method))
{
    (: Analyze a compound statement built from the statements and declarations 
       in the method. :)
    let $compound := <CompoundStatement> {
      for $content in $method/*
      let $name := local-name($content)
      where $name != 'AttributeList' and $name != 'ParameterDeclaration'
      return $content
    }</CompoundStatement>
    
    return local:visitCompoundStatement($compound)[1]
};

(: Visit any statement. A pair of (callsnext, breaksflow) is returned :)
declare function local:visitStatement($statement)
{
   typeswitch($statement)
      case element(EmptyStatement) return ($NOCALLSNEXT, $NOBREAKSFLOW)      
      case element(WhileStatement) return local:visitWhileStatement($statement)
      case element(DoWhileStatement) return local:visitDoWhileStatement($statement)
      case element(IfStatement) return local:visitIfStatement($statement)      
      case element(IfThenElseStatement) return local:visitIfThenElseStatement($statement)
      case element(ForStatement) return local:visitForStatement($statement)
      case element(SwitchStatement) return local:visitSwitchStatement($statement)
      case element(ExpressionStatement) return (local:visitExpression($statement/*[1]), $NOBREAKSFLOW)
      case element(CompoundStatement) return local:visitCompoundStatement($statement)
      case element(AssignEqualStatement)
         | element(AssignDecrementStatement)
         | element(AssignIncrementStatement) return local:visitAssignmentBinary($statement)
      case element(AssignMultipleFieldsStatement) return local:visitMultipleAssignment($statement)
      case element(ReturnStatement) return local:visitReturnStatement($statement)
      case element(UsingStatement) return local:visitUsingStatement($statement)
      case element(TryStatement) return local:visitTryStatement($statement)
      case element(LocalDeclarationsStatement) return local:visitLocalDeclarationsStatement($statement)
      case element(PrintStatement) return local:visitPrintStatement($statement)
      case element(UncheckedStatement)
         | element(ChangeCompanyStatement) return local:visitChangeOrUncheckedStatement($statement)
      case element(BreakStatement)       
         | element(ContinueStatement) 
         | element(RetryStatement) return ($NOCALLSNEXT, $BREAKSFLOW) 
      case element(ThrowStatement) return (local:visitExpression($statement/*[1]), $BREAKSFLOW)   
      case element(FindStatement)
         | element(DeleteStatement) return (local:visitQuery($statement/*[1]), $NOBREAKSFLOW) 
      case element(SearchStatement) return local:visitSearchStatement($statement)
      case element(InsertStatement) return local:visitInsertStatement($statement)
      case element(AssignPostIncrementStatement)
         | element(AssignPreIncrementStatement)
         | element(AssingPostDecrementStatement)
         | element(AssignPreDecrementStatement)  
         | element(BreakpointStatement) 
         | element(EmptyStatement) 
         | element(TtsAbortStatement)
         | element(TtsBeginStatement)
         | element(TtsEndStatement) 
         | element(FlushStatement) 
         | element(MoveCursorStatement) return ($NOCALLSNEXT, $NOBREAKSFLOW)    
      default (: All statements should be covered above. THe default is just a panic reaction :) 
        return (if ($statement//NextExpression) then $CALLSNEXT else $NOCALLSNEXT, $NOBREAKSFLOW)
};

(: Visit a list of expressions. :)
declare function local:visitExpressionList($exprs)
{
  local:visitExpressionListSofar($exprs, $NOCALLSNEXT)
};

declare function local:visitExpressionListSofar($exprs, $sofar)
{
  if (count($exprs) = 0) then (: end of list, return aggregated value :)
       $sofar
  else (
    let $exprStatus := local:visitExpression($exprs[1])
    
    return if ($exprStatus = $CALLSNEXT) then (: this element calls next => we're done :)
        $CALLSNEXT
    else (
      let $tail := remove($exprs, 1) (: recurse over rest of list :)
      return local:visitExpressionListSofar($tail, if ($sofar = $MAYCALLNEXT) then $MAYCALLNEXT else $exprStatus)
    )
  )
};

(: Visits the expression. Returns the calls next status. :)
declare function local:visitExpression($expression) 
{
   typeswitch($expression)
    case element(NextExpression) return $CALLSNEXT (: obviously :)
    case element(ConditionalExpression) return local:visitConditionalExpression($expression)
    case element(AndExpression) 
       | element(OrExpression) return local:visitAndOrExpression($expression)
    case element(AddExpression)    
       | element(DivideExpression) 
       | element(InExpression) 
       | element(IntegerDivideExpression)             
       | element(ModExpression)             
       | element(MultiplyExpression)             
       | element(PhysicalAndExpression)             
       | element(PhysicalOrExpression)             
       | element(PhysicalXorExpression)             
       | element(ShiftLeftExpression)             
       | element(ShiftRightExpression)             
       | element(SubtractExpression) 
       | element(EqualExpression)     
       | element(NotEqualExpression) 
       | element(GreaterThanExpression) 
       | element(GreaterThanOrEqualExpression) 
       | element(LessThanExpression) 
       | element(LessThanOrEqualExpression) 
       | element(LikeExpression) return local:visitBinaryExpression($expression)
    case element(SuperCall)
       | element(NewCall)
       | element(FunctionCall) 
       | element(StaticMethodCall)
       | element(NewClrCall)  return local:visitEvaluation($expression)
    case element(QualifiedCall) 
       | element(QualifiedStaticCall) return local:visitQualifiedCall($expression/*)
    case element(ContainerLiteralExpression) return local:visitExpressionList($expression/*)
    case element(FieldExpression) return local:visitFieldExpression($expression)
    case element(IntLiteralExpression) 
       | element(Int64LiteralExpression) 
       | element(StringLiteralExpression) 
       | element(RealLiteralExpression) 
       | element(BooleanLiteralExpression)
       | element(DateLiteralExpression)
       | element(DateTimeLiteralExpression)
       | element(NullLiteralExpression) return $NOCALLSNEXT
       
    default return if ($expression//NextExpression) then $CALLSNEXT else $NOCALLSNEXT
};

(: visit expressions containing a left and a right hand side :)
declare function local:visitBinaryExpression($expr)
{
  let $leftStatus := local:visitExpression($expr/*[1])
  
  return if ($leftStatus = $CALLSNEXT) then
    $CALLSNEXT 
  else (
    let $rightStatus := local:visitExpression($expr/*[2])
    return if ($rightStatus = $CALLSNEXT) then
        $CALLSNEXT
    else if ($leftStatus = $NOCALLSNEXT and $rightStatus = $NOCALLSNEXT) then
        $NOCALLSNEXT
    else $MAYCALLNEXT
  )
};

(: The ternary conditional expression (the ?: operator) is special. It calls 
   next if either the condition calls next, or both the thenpart and the 
   elsepart call next :)
declare function local:visitConditionalExpression($ce as element(ConditionalExpression))
{
  let $conditionStatus := local:visitExpression($ce/*[1])
  return if ($conditionStatus = $CALLSNEXT) then
     $CALLSNEXT
  else (
    let $truepartStatus := local:visitExpression($ce/*[2])
    let $falsepartStatus := local:visitExpression($ce/*[3])
    
    (: The conditional expression calls next if the condition calls next or if both the if part and false parts call next. 
       If none of the expressions call next then the conditional expression does not call next. In the remaining cases,  
       the conditional may call next  :)
    return if ($truepartStatus = $CALLSNEXT and $falsepartStatus = $CALLSNEXT) then
        $CALLSNEXT
    else if ($conditionStatus = $NOCALLSNEXT and $truepartStatus = $NOCALLSNEXT and $falsepartStatus = $NOCALLSNEXT) then
        $NOCALLSNEXT
  else $MAYCALLNEXT    
  )
};

(: The && and || operators are short circuit operators: The first expression is 
   always evaluated, but the second may not be :)
declare function local:visitAndOrExpression($expr)
{
  let $leftStatus := local:visitExpression($expr/*[1])
  return if ($leftStatus = ($CALLSNEXT, $MAYCALLNEXT)) then
    $leftStatus
  else (
    let $rightStatus := local:visitExpression($expr/*[2])
    return if ($rightStatus = $NOCALLSNEXT) then
      $NOCALLSNEXT
    else $MAYCALLNEXT 
  )
};

declare function local:visitFieldExpression($field as element(FieldExpression))
{
  local:visitFieldSpecification($field/*[1])
};

declare function local:visitFieldSpecification($field)
{
  let $status := if ($field/(ExpressionQualifier | SimpleQualifier)) then (
    let $qualifierStatus := local:visitExpressionQualifier($field/*[1])
    
    let $indexStatus := if (count($field/*) = 2) then
      local:visitExpression($field/*[2])
    else $NOCALLSNEXT
      
    return if ($qualifierStatus = $CALLSNEXT or $indexStatus = $CALLSNEXT) then
       $CALLSNEXT
    else if ($qualifierStatus = $NOCALLSNEXT and $indexStatus = $NOCALLSNEXT) then
       $NOCALLSNEXT
    else $MAYCALLNEXT
  )
  else (
    (: There was no qualifier. Is there an expression? :)
    if ($field/*) then local:visitExpression($field/*[1]) else $NOCALLSNEXT
  )
  
  let $qualifierStatus := typeswitch($field)
    case element(QualifiedField)
       | element(QualifiedStaticField) 
       | element(SimpleField)
       | element(StaticField) return $status
    case element(QualifiedNumberedField) (: field.(expr) :)
       return (
         let $numberedFieldStatus := local:visitQualifiedNumberedField($field)
         return if ($numberedFieldStatus = $CALLSNEXT) then
            $CALLSNEXT
         else if ($numberedFieldStatus = $NOCALLSNEXT and $status = $NOCALLSNEXT) then
            $NOCALLSNEXT
         else $MAYCALLNEXT
       ) 
    default return "Not implemented - Field specification: " || local-name($field)
    
  return $qualifierStatus
};

declare function local:visitExpressionQualifier($qualifier)
{
  typeswitch($qualifier)
    case element(ExpressionQualifier) return local:visitExpression($qualifier/*[1])
    case element(SimpleQualifier) 
       | element(StaticQualifier) return $NOCALLSNEXT
    default return "Not implemented - Expression qualifier: " || local-name($qualifier)
};

declare function local:visitQualifiedNumberedField($field as element(QualifiedNumberedField))
{
  local:visitExpression($field/*[1])
};

(: QualifiedCall or StaticQualifiedCall: a.foo(1,2) or T::f(1,2) :)
declare function local:visitQualifiedCall($c)
{   
  (: list of actual parameter values, followed by an Qualifier :)
  let $actuals := (for $a in $c[position() < last()] return $a)
  let $actualsStatus := local:visitExpressionList($actuals)

  return if ($actualsStatus = $CALLSNEXT) then
    $CALLSNEXT
  else (
    let $qualifier := $c[last()]
    
    let $qualifierStatus := local:visitExpressionQualifier($qualifier)
    return if ($qualifierStatus = $CALLSNEXT) then
      $CALLSNEXT
    else if ($actualsStatus = $NOCALLSNEXT and $qualifierStatus = $NOCALLSNEXT) then
      $NOCALLSNEXT
    else $MAYCALLNEXT 
  )
};

declare function local:visitEvaluation($e)
{ (: Visit the parameters :)
  local:visitExpressionList($e/*)
};

(: Visit a compound statement { ... } :)
declare function local:visitCompoundStatement($cs as element(CompoundStatement))
{
  local:visitStatementsInSequence($cs/*, $NOCALLSNEXT)
};

declare function local:visitStatementsInSequence($stmts, $sofar)
{
  if (count($stmts) = 0) then (: end recursion, return aggregated value :)
      ($sofar, $NOBREAKSFLOW)
  else (
    let $statementStatus := local:visitStatement($stmts[1])
    let $currentStatus := if ($statementStatus = $MAYCALLNEXT) then $MAYCALLNEXT else $sofar
    return if ($statementStatus[1] = $CALLSNEXT) then (: found one, no need to go any further :)
      ($CALLSNEXT, $NOBREAKSFLOW)
    else if ($statementStatus[2] = $BREAKSFLOW) then
      ($currentStatus, $NOBREAKSFLOW)
    else
       local:visitStatementsInSequence(
         remove($stmts, 1), $currentStatus)
  )
};

declare function local:visitWhileStatement($s as element(WhileStatement))
{
  let $conditionStatus := local:visitExpression( $s/*[1])
  
  let $callsNext := if ($conditionStatus = $CALLSNEXT or $conditionStatus = $MAYCALLNEXT) then
      $conditionStatus
  else  (
    let $statementStatus := local:visitStatement($s/*[2])
    
    return if ($statementStatus[1] = $NOCALLSNEXT) then
      $NOCALLSNEXT
    else 
      $MAYCALLNEXT
  )
  
  return ($callsNext, $NOBREAKSFLOW)
};

declare function local:visitDoWhileStatement($s as element(DoWhileStatement))
{
  let $conditionStatus := local:visitExpression($s/*[1])
  
  let $callsNext := if ($conditionStatus = $CALLSNEXT) then
      $CALLSNEXT
  else (
      let $statementStatus := local:visitStatement($s/*[2])

      return if ($statementStatus[1] = $CALLSNEXT) then
        $CALLSNEXT
      else  if ($conditionStatus = $NOCALLSNEXT and $statementStatus[1] = $NOCALLSNEXT) then
        $NOCALLSNEXT
      else $MAYCALLNEXT
  )
  return ($callsNext, $NOBREAKSFLOW)
};

declare function local:visitIfStatement($s as element(IfStatement))
{
  let $conditionStatus := local:visitExpression($s/*[1])
  let $status := if ($conditionStatus = $CALLSNEXT) then
    $CALLSNEXT
  else (
    let $truePartStatus := local:visitStatement($s/*[2])[1]
    return if ($conditionStatus = $NOCALLSNEXT and $truePartStatus = $NOCALLSNEXT) then
      $NOCALLSNEXT
    else $MAYCALLNEXT
  )
  
  return ($status, $NOBREAKSFLOW)
};

declare function local:visitIfThenElseStatement($s as element(IfThenElseStatement))
{
  let $exprStatus := local:visitExpression($s/*[1])
  
  let $status := if ($exprStatus = $CALLSNEXT) then
    $CALLSNEXT
  else (
    let $ifPartStatus := local:visitStatement($s/*[2])[1]
    let $elsePartStatus := local:visitStatement($s/*[3])[1]
      
    (: the statement calls next if either the condition calls next, or both the then part and the else part call next :)
    return if ($ifPartStatus = $CALLSNEXT and $elsePartStatus = $CALLSNEXT) then
      $CALLSNEXT 
    (: The statement does not call next If neither the condition, the if part and the else part call next :)
    else if ($exprStatus = $NOCALLSNEXT and $ifPartStatus = $NOCALLSNEXT and $elsePartStatus = $NOCALLSNEXT) then
      $NOCALLSNEXT
    else $MAYCALLNEXT
  )
  
  return ($status, $NOBREAKSFLOW)
};

declare function local:visitUsingStatement($s as element(UsingStatement))
{
   let $usingStatus := local:visitVariableDeclarations($s/VariableDeclaration, $NOCALLSNEXT)
   let $status := if ($usingStatus = $CALLSNEXT) then
      $CALLSNEXT
   else (
     let $statementStatus := local:visitStatement($s/*[last()])
     return if ($statementStatus = $CALLSNEXT) then
        $CALLSNEXT
     else (
       if ($statementStatus = $NOCALLSNEXT and $usingStatus = $NOCALLSNEXT) then
          $NOCALLSNEXT
       else $MAYCALLNEXT
     ) 
   )
   return ($status, $NOBREAKSFLOW)
};

declare function local:visitTryStatement($s as element(TryStatement))
{
  let $statementStatus := local:visitStatement($s/*[1])
  return if ($statementStatus[1] = $CALLSNEXT) then 
    ($CALLSNEXT, $NOBREAKSFLOW)
  else (
    let $finally := if (count($s/*) mod 2 = 0) then $s/*[last()] else <EmptyStatement/>
    let $finallyStatus := local:visitStatement($finally)
    
    return if ($finallyStatus[1] = $CALLSNEXT) then
        ($CALLSNEXT, $NOBREAKSFLOW)
    else (
      (: Construct a dummy list of statement without the catches :)
      let $handlers := $s/(CatchAllValues | CatchExpression)/following-sibling::*[1]
      let $catchExpressions := $s/CatchExpression/*
    
      let $handlersStatus := local:visitStatementsInSequence($handlers, $NOCALLSNEXT)
      let $catchExpressionsStatus := local:visitExpressionList($s/*)
      
      return if ($statementStatus[1] = $NOCALLSNEXT and $finallyStatus[1] = $NOCALLSNEXT 
             and $handlersStatus[1] = $NOCALLSNEXT and $catchExpressionsStatus = $NOCALLSNEXT) then
        ($NOCALLSNEXT, $NOBREAKSFLOW)
      else ($MAYCALLNEXT, $NOBREAKSFLOW)
    )
  )
};

(: for(initialization; condition; update) stmt :)
declare function local:visitForStatement($s as element(ForStatement))
{
  let $initializationStatus := local:visitForAssign($s/*[1])
  let $status := if ($initializationStatus = $CALLSNEXT) then
     $CALLSNEXT
  else (
    let $conditionStatus := local:visitExpression($s/*[2])
    
    return if ($conditionStatus = $CALLSNEXT) then
        $CALLSNEXT
    else (
      let $updateStatus := local:visitForAssign($s/*[3])
      let $statementStatus := local:visitStatement($s/*[4])
      return if ( $initializationStatus = $NOCALLSNEXT 
              and $conditionStatus = $NOCALLSNEXT 
              and $updateStatus = $NOCALLSNEXT
              and $statementStatus[1] = $NOCALLSNEXT) then
         $NOCALLSNEXT
      else $MAYCALLNEXT
    )
  )
  return ($status, $NOBREAKSFLOW)
};

declare function local:visitSwitchStatement($s as element(SwitchStatement))
{
  let $switchValueStatus := local:visitExpression($s/*[1])
  let $status := if ($switchValueStatus = $CALLSNEXT) then
    $CALLSNEXT
  else (
    (: TODO implement this properly. It can be done by transforming the 
       switch statement into an if statement with the same semantics and 
       then analysing that statement. So for instance:
       
       switch(e){
         case e1,e2, e3:
         case e4 : S11; S12
         case e5 : S2
         default: S3
       }
       can be generated into
       
       if (e1 || e2 || e3 || e4)
       { S11; S12 }
       else if (e5)
       { S2l }
       else { S3 }
       :)
     $NOCALLSNEXT
  )
  return ($status, $NOBREAKSFLOW)
};

declare function local:visitForAssign($f)
{
    typeswitch($f)
      case (: field += expr  :) element(ForFieldIncrementAssign) 
         | (: field -= expr  :) element(ForFieldDecrementAssign) 
         | (: field = expr   :) element(ForFieldAssign) return local:visitExpression($f/*[2]) 
      case (: field++        :) element(ForFieldPostIncrement)
         | (: field--        :) element(ForFieldPostDecrement)   
         | (: ++field        :) element(ForFieldPreIncrement) 
         | (: --field        :) element(ForFieldPreDecrement) return $NOCALLSNEXT
      case (: T f1=e, T f2=e :) element(ForDeclarationAssign) return local:visitVariableDeclarations($f/VariableDeclaration, $NOCALLSNEXT)
      default return $NOCALLSNEXT
};

declare function local:visitPrintStatement($s as element(PrintStatement))
{
  (local:visitExpressionList($s/*), $NOBREAKSFLOW)
};

declare function local:visitChangeOrUncheckedStatement($s)
{
  let $expressionStatus := local:visitExpression($s/*[1])
  
  return if ($expressionStatus = $CALLSNEXT) then
    ($CALLSNEXT, $NOBREAKSFLOW)
  else (
    let $statementStatus := local:visitStatement($s/*[2])[1]
  
    return if ($statementStatus = $CALLSNEXT) then
     ($CALLSNEXT, $NOBREAKSFLOW)
    else if ($expressionStatus = $NOCALLSNEXT and $statementStatus = $NOCALLSNEXT) then
     ($NOCALLSNEXT, $NOBREAKSFLOW) 
    else ($MAYCALLNEXT, $NOBREAKSFLOW) 
  )
};

declare function local:visitReturnStatement($s as element(ReturnStatement))
{
  let $status := if ($s/*[1]) then local:visitExpression($s/*[1]) else $NOCALLSNEXT
  return ($status, $BREAKSFLOW)
};

declare function local:visitAssignmentBinary($s)
{
  (: Binary assignments contain two children: The field assigned to and the expression :)
  (local:visitExpression($s/*[2]), $NOBREAKSFLOW)
};

(: [a,b,c] = container :)
declare function local:visitMultipleAssignment($s)
{
  local:visitExpression($s/*[last()])
};

(: A variable declaration can contain several declaratons:
   int i = 9, j = next foo(); s = "Banana";
   We deliverately discard function declarations
:)
declare function local:visitLocalDeclarationsStatement($s as element(LocalDeclarationsStatement))
{
  (local:visitVariableDeclarations($s/VariableDeclaration, $NOCALLSNEXT), $NOBREAKSFLOW)
};

declare function local:visitVariableDeclarations ($s as element(VariableDeclaration)*, $sofar)
{
  if (count($s) = 0) then (: end of list, so stop recursion returning aggregate value :)
     $sofar
  else (
    let $status := local:visitVariableDeclaration($s[1])
    
    return if ($status = $CALLSNEXT) then
      $CALLSNEXT
    else ( (:recurse with the rest of the list :)
      let $tail := remove($s, 1)
      return local:visitVariableDeclarations($tail, if ($status = $MAYCALLNEXT) then $MAYCALLNEXT else $sofar)
    )
  )
};

declare function local:visitVariableDeclaration($s as element(VariableDeclaration))
{
  (: A declaration can be either just the type or the type and an initializer :)
  if (count($s/*) = 1) then (: No initialization expression :)
     $NOCALLSNEXT
  else local:visitExpression($s/*[2])
};

declare function local:visitQuery($q as element(Query))
{
  let $filtered := $q/*[not (local-name(.) = (
    "ExplicitSelection", "ImplicitSelection",
    "ValidTimeStateDate", "ValidTimeStateRange",
    "CrossCompanyAll",
    "SimpleOrderElement"))]
  
  let $crossCompanyContainer := $filtered[local-name(.) = "CrossCompanyContainer"]
  
  let $crossCompanyStatus := if ($crossCompanyContainer) then
      local:visitExpression($crossCompanyContainer/*[1])
  else $NOCALLSNEXT
  
  let $onlyWhere := $filtered[local-name(.) != 'JoinSpecification']
  
  let $whereStatus := if ($onlyWhere) then
      local:visitExpression($onlyWhere)
  else $NOCALLSNEXT
  
  let $joinSpec := $filtered[local-name(.) = 'JoinSpecification']
 
  let $joinContribution := if ($joinSpec) then
      local:visitQuery($joinSpec/*)
  else $NOCALLSNEXT
  
  return if ($crossCompanyStatus = $CALLSNEXT or $whereStatus = $CALLSNEXT or $joinContribution = $CALLSNEXT) then
    $CALLSNEXT
  else if ($crossCompanyStatus = $NOCALLSNEXT and $whereStatus = $NOCALLSNEXT and $joinContribution = $NOCALLSNEXT) then
    $NOCALLSNEXT
  else $MAYCALLNEXT
  
};

declare function local:visitSearchStatement($s as element(SearchStatement))
{
  let $queryStatus := local:visitQuery($s/*[1])
  
  return (if ($queryStatus = $CALLSNEXT) then
    $CALLSNEXT
  else (
    let $statementStatus := local:visitStatement($s/*[2])[1]

    return if ($queryStatus = $NOCALLSNEXT and $statementStatus = $NOCALLSNEXT) then
       $NOCALLSNEXT
    else $MAYCALLNEXT, $NOBREAKSFLOW)
  )
};

declare function local:visitInsertStatement($s as element(InsertStatement))
{
  (local:visitQuery($s/*[1]), $NOBREAKSFLOW)
};

declare function local:visitUpdateStatement($s as element(UpdateStatement))
{
  (: Todo. Currently there is no information about the field assignments :)
  ($NOCALLSNEXT, $NOBREAKSFLOW)
};

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/CallsNext' Version='0.1'>
{
  for $extensionClass in /Class[@ExtensionOfAttributeExists]
  for $m in $extensionClass/Method[@IsChainOfCommandMethod='true']    let $methodStatus := local:visitMethodContent($m)
    where $methodStatus != $CALLSNEXT
    return <Diagnostic Artifact='{$extensionClass/@Artifact}' 
        Method='{$m/@Name}' Package='{$extensionClass/@Package}'
        Status='{$methodStatus}'
        StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}' />
    (: <Diagnostic>
      <Moniker>CallsNextNotProvable</Moniker>
      <Severity>Error</Severity>
      <Path>{string($extensionClass/@PathPrefix)}/Method/{string($m/@Name)}</Path>
      <Message>Not all paths contain a call to next in this extension method.</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($m/@StartLine)}</Line>
      <Column>{string($m/@StartCol)}</Column>
      <EndLine>{string($m/@EndLine)}</EndLine>
      <EndColumn>{string($m/@EndCol)}</EndColumn>
    </Diagnostic> :)  
}
</Diagnostics>
