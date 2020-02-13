(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide interesting metrics for classes. The metrics provided
follow the naming conventions used in the literature. :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

<ClassMetrics Category='Mandatory' href='docs.microsoft.com/Socratex/ClassMetrics' Version='1.0'>
{
    for $a in /(Class | Interface)
    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))
    order by $weightedMethodComplexity
    return <ClassMetric Artifact='{$a/@Artifact}' Package='{$a/@Package}'
        IsAbstract ='{$a/@IsAbstract}'
        IsInterface='{name($a) = 'Interface'}'
        Extends='{$a/@Extends}'
        NOAM='{count($a/Method[@IsAbstract="true"])}'
        LOC='{$a/@EndLine - $a/@StartLine + 1}'
        NOM='{count($a/Method)}'
        NOA='{count($a/FieldDeclaration)}'
        WMC='{$weightedMethodComplexity}'
        NOPM='{count($a/Method[@IsPublic="true"])}'
        NOPA='{count($a/FieldDeclatation[@IsPublic="true"])}'
        NOS='{count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt)}'
        StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</ClassMetrics>