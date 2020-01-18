(: Find the class definitions in the database :)
<Classes>
{
    for $m in /Module
    for $c in $m/Statements/Class
        return <ClassDef Artifact='{$m/@Artifact}' Name='{$c/@Name}' Language='Python'
                         StartLine='{$c/@StartLine}' StartCol='{$c/@StartCol}'/>
}
</Classes>