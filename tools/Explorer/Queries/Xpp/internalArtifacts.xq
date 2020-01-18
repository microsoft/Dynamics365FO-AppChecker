(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds all internal methods :)
(: @Category Informational :)
(: @Language Xpp :)

<InternalArtifacts>
{
for $a in /(Class | Table | Query | Form )[@Modifiers="InternalModifier"]
return <InternalArtifact Artifact='{$a/@Artifact}' 
    StartLine='{$a/@StartLine}' EndLine='{$a/@EndLine}'
    StartCol='{$a/@StartCol}' EndCol='{$a/@EndCol}' />
}
</InternalArtifacts>
