(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Finds places where prmIsDefault is used. :)

(: @Category Informational :)
(: @Language Xpp :)

let $c := <PrmIsDefaults>
{
  for $a in /(Class | Table | Form | Query)[@Package='ApplicationSuite']
  for $m in $a//Method[(starts-with(lower-case(@Name), 'parm'))]
  for $prmDefault in $m//FunctionCall[lower-case(@FunctionName) = 'prmisdefault']
  return <PrmIsDefault Artifact='{$a/@Artifact}' Method='{$m/@Name}' 
      StartLine='{$prmDefault/@StartLine}' EndLine='{$prmDefault/@EndLine}'
      StartCol='{$prmDefault/@StartCol}' EndCol='{$prmDefault/@EndCol}'  />
}</PrmIsDefaults>

return $c