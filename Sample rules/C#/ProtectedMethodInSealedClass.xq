(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: CA1047: Do not declare protected members in sealed types :)
(: @Category BestPractice :)
(: @Language C# :)

<CA1047>
{
    for $c in //ClassDeclaration[@IsSealed='true']
    for $m in $c/MethodDeclaration[@DeclaredAccessibility='Protected']
    return <ProtectedMethodInSealedClass ClassName='{$c/@Name}' MethodName='{$m/@Name}' />
}
</CA1047>