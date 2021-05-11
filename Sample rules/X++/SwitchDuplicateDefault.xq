(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find switch statements that have more than one default part.
Having more than one default part is semantically correct in X++
but makes maintenance much more difficult :)

(: @Language Xpp :)
(: @Category Informational :)

<SwitchDuplicateDefault>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $s in $m//SwitchStatement
  where count($s/CaseDefault) > 1
  return <DuplicateDefault Artifact='{$a/@Artifact}' 
      StartLine='{$s/@StartLine}' EndLine='{$s/@EndLine}' 
      StartCol='{$s/@StartCol}' EndCol='{$s/@EndCol}' />  
}
</SwitchDuplicateDefault>