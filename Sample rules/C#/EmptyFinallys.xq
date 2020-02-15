(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify try / finally statements with empty finally blocks :)
(: @Category BestPractice :)
(: @Language C# :)

<EmptyFinallys>
{
    for $c in //ClassDeclaration
    for $m in $c//MethodDeclaration
    for $ts in $m//TryStatement
    where not ($ts/FinallyClause/Block/*)
    return <Res Artifact='{$c/@Artifact}' Language="C#" Method='{$m/@Name}'
                StartLine='{$ts/@StartLine}' StartCol='{$ts/@StartCol}'
                EndLine='{$ts/@EndLine}' EndCol='{$ts/@EndCol}' />

}
</EmptyFinallys>