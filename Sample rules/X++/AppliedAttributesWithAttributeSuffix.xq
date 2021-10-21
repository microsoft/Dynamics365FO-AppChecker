(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find references to the application of attributes 
   where the Attribute suffix is included in the attribute name. :)

(: @Language Xpp :)
(: @Category Informational :)

<AppliedAttributesWithAttributeSuffix>
{
  for $a in /(Class | Table | Query | Form)
  for $attr in $a//Attribute 
  where ends-with(lower-case($attr/@Name), 'attribute')
  return <AppliedAttributesWithAttributeSuffix Artifact='{$a/@Artifact}'
      StartLine='{$attr/@StartLine}' EndLine='{$attr/@EndLine}' StartCol='{$attr/@StartCol}' EndCol='{$attr/@EndCol}' />
}
</AppliedAttributesWithAttributeSuffix>
