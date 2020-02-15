(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: determines the fields of a particular visibility, here private :)
(: @Category Informational :)
(: @Language Xpp :)

<Results>
{
  for $c in /Class
  for $field in $c/FieldDeclaration[@IsPrivate='True'] (: use other attributes for other visibilities :)
  return <Result Artifact='{$c/@Artifact}' FieldName='{$field/@Name}'
     StartLine='{$field/@StartLine}' StartCol='{$field/@StartCol}' 
     EndLine='{$field/@EndLine}' EndCol='{$field/@EndCol}' />
}
</Results>