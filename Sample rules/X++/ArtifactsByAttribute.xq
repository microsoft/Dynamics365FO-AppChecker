(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: For every attribute, get the artifacts decorated with that attribute :)
(: @Category Informational :)
(: @Language Xpp :)

let $a := <ArtifactsWithAttribute  Category='Informational' href='docs.microsoft.com/Socratex/ArtifactWithAttribute' Version='1.0'>
{
  for $a in /(Class | Table | Form)
  for $attr in $a/AttributeList/Attribute
  where $attr
  order by $a/@Name
  return <ArtifactWithAttribute Artifact='{$a/@Artifact}' AttributeName='{$attr/@Name}'
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</ArtifactsWithAttribute>

return <ArtifactsByAttribute>
{
    for $attributeName in distinct-values($a/ArtifactWithAttribute/@AttributeName)
    order by $attributeName
    return <Attribute Name='{$attributeName}' >
    {
        for $qqq in $a/ArtifactWithAttribute[@AttributeName=$attributeName]
        let $artifactName := $qqq/@Artifact
        order by $artifactName
        return <Artifact Artifact='{$artifactName}' StartLine='{$qqq/@StartLine}' EndLine='{$qqq/@StartLine}' />
    }
    </Attribute>
}
</ArtifactsByAttribute>
