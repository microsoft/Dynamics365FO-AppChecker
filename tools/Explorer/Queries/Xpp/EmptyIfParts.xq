(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Returns ifstatements that have empty constituents. :)
(: This is different from empty compound statements, because the offending
   statement can be have either empty compound statements, or empty statement
   as its children :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<EmptyIfParts Category='Mandatory' href='docs.microsoft.com/Socratex/EmptyIfParts' Version='1.0'>
{
for $c in /*
for $m in $c//Method
for $e in $m//(IfStatement | IfThenElseStatement)/(CompoundStatement | EmptyStatement)
   where not($e/*)
   return <EmptyIfPart Artifact='{$c/@Artifact}' Method='{$m/@Name}' 
      StartLine='{$e/@StartLine}' StartCol='{$e/@StartCol}'
      EndLine='{$e/@EndLine}' EndCol='{$e/@EndCol}'/>
}
</EmptyIfParts>