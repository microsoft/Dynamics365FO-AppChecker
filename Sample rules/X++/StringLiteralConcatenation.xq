(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Indentify places where string literals are concatenated :)
(: This is wasteful and should be replaced with the complete literal :)
(: @Category Performance :)
(: @Language Xpp :)

<Classes>
{
    for $c in /*
    for $m in $c//Method//AddExpression
    
    where $m/StringLiteralExpression[1] and $m/StringLiteralExpression[2]
    
    return <Res Artifact='{$c/@Artifact}' 
                StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}'
                EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />}
</Classes>
