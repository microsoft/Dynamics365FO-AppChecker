(: Find all empty methods :)
<Results>
{
    for $c in /Class | /Table | /Form | /Query
    for $m in $c//Method
    let $attrs := count($m/AttributeList)
    let $parmsdecls := count($m/ParameterDeclaration)
    where $m[@IsAbstract='False'] and (count($m/*) - $attrs - $parmsdecls) = 0
    return <Result Artifact='{$c/@Artifact}'
            StartLine='{$m/@StartLine}'  StartCol='{$m/@StartCol}'
            EndLine='{$m/@EndLine}'  EndCol='{$m/@EndCol}' />
}
</Results>