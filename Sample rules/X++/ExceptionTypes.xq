(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Shows the types of exceptions handled in catch blocks :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
    for $a in /(Class | Form | Query | Table)
    for $m in $a//Method
    for $catchExpr in $m//TryStatement/CatchExpression/*[1]
    where $catchExpr/@Type != "Enumeration(Exception)" and $catchExpr/@Type != "System.Exception" 
    return <Result Artifact='{$a/@Artifact}' Type='{$catchExpr/@Type}'
        StartLine='{$catchExpr/@StartLine}' StartCol='{$catchExpr/@StartCol}'
        EndLine='{$catchExpr/@EndLine}' EndCol='{$catchExpr/@EndCol}' />
    
}
</Results>
