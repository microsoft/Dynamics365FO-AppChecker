(: CA1047: Do not declare protected members in sealed types :)
(: Licensed under the MIT license. :)

(: Identify protected methods in final classes :)
(: @Category BestPractice :)
(: @Language X++ :)

<CA1047>
{
    for $c in /Class[@IsFinal='true'][@Package='ApplicationSuite']
    for $m in $c/Method[lower-case(@IsFinal)='true']
    return <ProtectedMethodInFinalClass ClassName='{$c/@Name}'
        MethodName='{$m/@Name}'
        StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}'
        StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}'/>
}
</CA1047>