(: Classes with more than 20 fields ordered by their number of fields. :)
<Classes>
{
    let $limit := 20
    for $c in /Class 
    where count($c/FieldDeclaration) > $limit
    order by count($c/FieldDeclaration) descending
    return <Result Artifact='{$c/@Artifact}' Fields='{count($c/FieldDeclaration)}'/>
}
</Classes>