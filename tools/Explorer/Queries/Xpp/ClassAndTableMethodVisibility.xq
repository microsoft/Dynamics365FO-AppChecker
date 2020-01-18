(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Calculates the visibility of all methods on classes and tables. :)
(: @Language Xpp :)
(: @Category Informational :)
(: @Author pvillads@microsoft.com :)

let $results :=
<Results>
{
  for $c in /Class | /Table

    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'True'])
    let $protectedMethods := count($c/Method[@IsProtected = 'True'])
    let $publicMethods := count($c/Method[@IsPublic = 'True'])
    let $internalMethods := count($c/Method[@IsInternal = 'True'])
    let $undecoratedMethods := count($c/Method[@IsInternal='False' and @IsPrivate='False' and @IsProtected='False' and @IsPublic='False'])

    return <Result Artifact='{$c/@Artifact}'
        PrivateMethodCount='{$privateMethods}'
        ProtectedMethodCount='{$protectedMethods}'
        PublicMethodCount='{$publicMethods}'
        InternalMethodCount='{$internalMethods}'
        UnDecoratedMethodCount='{$undecoratedMethods}'/>
}
</Results>

return $results