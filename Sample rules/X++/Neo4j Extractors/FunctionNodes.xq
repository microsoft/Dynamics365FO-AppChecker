(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export embedded function information in CSV format :)

declare function local:MethodComplexity($m) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

declare function local:Record($artifactString, $entity, $method, $function)
{
    <Record>
        <Artifact name='Artifact:ID'>{lower-case($artifactString)}</Artifact>
        <StartLine name='StartLine:int'>{xs:integer($function/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($function/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($function/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($function/@EndCol)}</EndCol>
        <CMP name='CMP:int'>{local:MethodComplexity($function)}</CMP>
        <LOC name='LOC:int'>{$function/@EndLine - $function/@StartLine + 1}</LOC>
        <NOS name='NOS:int'>{count(for $stmt in $function//* where ends-with(name($stmt), 'Statement') return $stmt)}</NOS>
        <Label name=':LABEL'>Function</Label>
    </Record>
};

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <LocalFunctions>
{
    (
    for $c in /Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/classes/' || $c/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Entity
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/dataentityviews/' || $c/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Form/Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/../@Package || '/forms/' || $c/../@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Form
    for $control in $c/FormControl
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/forms/' || $c/@Name || '/controls/' || $control/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Form
    for $control in $c/FormDataSource
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/forms/' || $c/@Name || '/datasources/' || $control/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Map
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/maps/' || $c/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Query/Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/../@Package || '/queries/' || $c/../@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /View
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/views/' || $c/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm),

    for $c in /Table
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $p := '/' || $c/@Package || '/tables/' || $c/@Name || '/methods/' || $m/@Name || '/' || $lm/@Name
    return local:Record($p, $c, $m, $lm)
    )
}
</LocalFunctions>

return csv:serialize($r, $options)
