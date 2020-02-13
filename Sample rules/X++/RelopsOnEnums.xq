(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: This query shows all the non equality relational operators 
   that are applied on enumeration types. These places may cause 
   problems if any of the types are made extensible. You can always
   provide a filter on the name of the enum if you are looking for
   particular ones. :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<EnumComparisons>
{
    for $a in /(Class | Table | Query | Form)
    for $e in $a//(GreaterThanExpression | GreaterThanOrEqualExpresssion 
                 | LessThanExpression | LessThanOrEqualsExpression)
    let $lhs := $e/*[1], $rhs := $e/*[2]
    where starts-with($lhs/@Type, "Enumeration") or starts-with($rhs/@Type, "Enumeration") 
    order by $a/@Package
    return <EnumComparison 
        Artifact='{$a/@Artifact}' Package='{$a/@Package}'
        LHT='{$lhs/@Type}'  RHT='{$rhs/@Type}'
        
        StartLine='{$e/@StartLine}' EndLine='{$e/@EndLine}'
        StartCol='{$e/@StartCol}' EndCol='{$e/@EndCol}' />
}
</EnumComparisons>