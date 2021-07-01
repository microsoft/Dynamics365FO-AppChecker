(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find switch statements that have a default part that is not the last case entry.
This is semantically correct in X++ but makes maintenance much more difficult :)

(: Note: This does not find references updated with byref parameters :)
(: @Language Xpp :)
(: @Category Informational :)

<SwitchDefaultNotFinals>
{
  for $a in /*
  for $s in $a//SwitchStatement
  for $defaultPart in $s/CaseDefault
  where $defaultPart/following-sibling::CaseValues or  $defaultPart/following-sibling::CaseDefault
  return <SwitchDefaultNotFinal Artifact='{$a/@Artifact}'
       StartLine='{$defaultPart/@StartLine}' EndLine='{$defaultPart/@EndLine}' 
       StartCol='{$defaultPart/@StartCol}' EndCol='{$defaultPart/@EndCol}'/>  

}
</SwitchDefaultNotFinals>
