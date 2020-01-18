(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find the class definitions in the database :)
(: @Language Python :)
<Classes>
{
    for $m in /Module
    for $c in $m/Statements/Class
        return <ClassDef Artifact='{$m/@Artifact}' Name='{$c/@Name}' Language='Python'
                         StartLine='{$c/@StartLine}' StartCol='{$c/@StartCol}'/>
}
</Classes>