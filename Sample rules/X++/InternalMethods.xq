(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds all internal methods :)
(: @Category Informational :)
(: @Language Xpp :)

<InternalMethods>
{
for $a in /(Class | Table | Form | Query)
for $m in $a//Method[@IsInternal='True']
return <InternalMethod Artifact='{$a/@Artifact}' Method='{$m/@Name}'
    StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}'
    StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}' />
}
</InternalMethods>
