(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find methods that have inline (local) functions declared :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<LocalFunctions>
{
    for $a in /(Class | Form | Table | Query)
    for $m in $a//Method
    for $f in $m/LocalDeclarationsStatement/FunctionDeclaration
    return <LocalFunction Artifact='{$a/@Artifact}' Method='{$m/@Name}' Name='{$f/@Name}'
      StartLine='{$f/@StartLine}' EndLine='{$f/@EndLine}'
      StartCol='{$f/@StartCol}' EndCol='{$f/@EndCol}' />      
    
}
</LocalFunctions>