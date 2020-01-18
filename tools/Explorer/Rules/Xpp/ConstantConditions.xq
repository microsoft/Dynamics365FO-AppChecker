(: Find conditions in if statements that are constants :)
<ConstantConditions>
{
  for $c in /*
  for $i in $c//(IfStatement | IfThenElseStatement | WhileStatement)/*[1][@IsConst='True']
  return <ConstantCondition Artifact='{$c/@Artifact}' StartLine='{$i/@StartLine}' StartCol='{$i/@StartCol}' EndLine='{$i/@EndLine}' EndCol='{$i/@EndCol}' />
}
</ConstantConditions