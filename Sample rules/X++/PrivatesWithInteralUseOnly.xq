(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get private artifacts decorated with an internaluseonly attribute:)
(: @Category Informational :)
(: @Language Xpp :)

<PrivateInternals>
{
    for $a in /Class
    for $m in $a//Method
    where $m/@IsPrivate = 'true'
    and $m/AttributeList/Attribute[lower-case(@Name) = 'internaluseonly']
    return <PrivateInternal Artifact='{$a/@Artifact}'
        StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}' />
}
</PrivateInternals>
