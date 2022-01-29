(: Copyright (c) Microsoft Corporation.
Licensed under the MIT license. :)

(: @Language Xpp :)
(: @Category Informational :)
<T>
{
  for $a in /(Class | Table | Query | Form)
  for $attr in $a//AttributeList/Attribute[count(*)>=2]
  for $tablestrattr in $attr/AttributeExpression/IntrinsicAttributeLiteral[lower-case(@FunctionName) = ('tablestr', 'classstr', 'querystr', 'formstr')]
  for $fieldstrattr in $attr/AttributeExpression/IntrinsicAttributeLiteral[lower-case(@FunctionName) = ('methodstr', 'formmethodstr', 'tablemethodstr', 'staticmethodstr', 'tablestaticstr', 'fieldstr')]
  where lower-case(tablestrattr/@Arg1) != lower-case(fielstrattr/@Arg1)
 
  order by $a/@Artifact
  return <Res Artifact='{$a/@Artifact}' 
    StartLine='{$attr/@StartLine}' StartCol='{$attr/@StartCol}'
    EndLine='{$attr/@EndLine}' EndCol='{$attr/@EndCol}' />
}
</T>
