(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify protected methods on final classes :)
(: @Language Xpp :)
(: @Category Optional :)
<ProtectedMethodsOnFinalClasses>
{
    for $c in /Class[@IsFinal='true']
    for $m in $c/Method[@IsProtected='true']
    return <ProtectedMethodOnFinalClass
        Artifact='{$c/@Artifact}'
        StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}' 
        StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}' />
}
</ProtectedMethodsOnFinalClasses>