(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get artifacts decorated with a particular attribute :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<ArtifactsWithAttribute  Category='Informational' href='docs.microsoft.com/Socratex/ArtifactWithAttribute' Version='1.0'>
{
  for $a in /(Class | Table | Form)
  where $a/AttributeList/Attribute[lower-case(@Name) = 'datacontractattribute']
  order by $a/@Name
  return <ArtifactWithAttribute Artifact='{$a/@Artifact}'
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</ArtifactsWithAttribute>