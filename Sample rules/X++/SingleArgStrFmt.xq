(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find references where strfmt is called with only one argument.
   These places should not call strfmt. :)
(: Note: This does not find references updated with byref parameters :)
(: @Language Xpp :)
(: @Category Informational :)

<StrFmtSingleArgs>
{
    for $a in /(Class | Table | Map | View | Query | Form)
    for $m in $a//Method
    for $call in $m//FunctionCall[lower-case(@FunctionName)='strfmt']
    where count($call/*) = 1
    order by $a/@Package, $a/@Name
    return <StrFmtSingleArg Package='{$a/@Package}' Artifact='{$a/@Artifact}'
        StartLine='{$call/@StartLine}' EndLine='{$call/@EndLine}' StartCol='{$call/@StartCol}' EndCol='{$call/@EndCol}'  />
}
</StrFmtSingleArgs>