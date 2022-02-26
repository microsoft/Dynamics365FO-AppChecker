(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify calls to tablename2id(TableStr(T)) that could base
expressed better with tableNum(T), since there is no runtime
function called in the latter case. :)

(: @Language Xpp :)
(: @Category Informational :)
<CallsTableName2IdInTableStr>
{
    for $a in /*
    for $m in $a//Method
    for $call in $m//FunctionCall[lower-case(@FunctionName)='tablename2id']/Intrinsic[lower-case(@Name)='tablestr']
    return <Call Artifact='{$a/@Artifact}'
        StartLine='{$call/@StartLine}' EndLine='{$call/@EndLine}'
        StartCol='{$call/@StartCol}' EndCol='{$call/@EndCol}'/>
}
</CallsTableName2IdInTableStr>
