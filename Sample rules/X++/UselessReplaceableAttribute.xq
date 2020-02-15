(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get artifacts decorated with a replaceable attribute that is meaningless :)
(: @Category Informational :)
(: @Language Xpp :)

<UselessReplaceableAttributes  Category='Informational' href='docs.microsoft.com/Socratex/MeaninglessReplaceableAttributes' Version='1.0'>
{
  for $a in /(Class | Table | Form)
  where $a//Method/AttributeList/Attribute[lower-case(@Name) = 'replaceable']//BooleanAttributeLiteral[@Value='false']
  order by $a/@Name
  return <UselessReplaceableAttribute Artifact='{$a/@Artifact}' 
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</UselessReplaceableAttributes>