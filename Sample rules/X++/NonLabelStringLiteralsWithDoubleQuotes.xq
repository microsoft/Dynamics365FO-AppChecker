(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find string literals that start with double quotes and are not labels :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
  for $c in (/Class | /Table | /Form | /Query)
  for $literal in $c//StringLiteralExpression (: [@IsLabel='False']:)
  let $value := $literal/@Value
  where starts-with($value, '"') and not (contains($value, '@')) and string-length(string($value)) > 2
  return <Result Artifact='{$c/@Artifact}' 
             StartLine='{$literal/@StartLine}' EndLine='{$literal/@EndLine}'
             StartCol='{$literal/@StartCol}' EndCol='{$literal/@EndCol}'>{string($value)}</Result>
}
</Results>