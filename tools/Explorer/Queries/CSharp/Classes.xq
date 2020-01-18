(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find the class definitions in the database :)
(: @Language C# :)
<Classes>
{
    for $c in //ClassDeclaration
    return <ClassDefinition Artifact='{$c/@Artifact}' FullName='{$c/@FullName}' Language='C#'
                  StartLine='{$c/@StartLine}' StartCol='{$c/@StartCol}'
				  EndLine='{$c/@EndLine}' EndCol='{$c/@EndCol}'/>
}
</Classes>