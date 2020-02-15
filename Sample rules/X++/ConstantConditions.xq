(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find conditions in if statements that are constants :)
(: @Category Mandatory :)
(: @Language Xpp :)

<ConstantConditions Category='Mandatory' href='docs.microsoft.com/Socratex/ConstantConditions' Version='1.0'>
{
  for $c in /*
  for $i in $c//(IfStatement | IfThenElseStatement | WhileStatement)/*[1][@IsConst='True']
  return <ConstantCondition Artifact='{$c/@Artifact}' StartLine='{$i/@StartLine}' StartCol='{$i/@StartCol}' EndLine='{$i/@EndLine}' EndCol='{$i/@EndCol}' />
}
</ConstantConditions>