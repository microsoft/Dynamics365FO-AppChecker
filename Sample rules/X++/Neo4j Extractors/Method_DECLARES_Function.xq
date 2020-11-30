(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export embedded function information in CSV format :)

declare function local:Record($methodName, $functionName)
{
    <Record>
        <From name=':START_ID'>{lower-case($methodName)}</From>
        <To name=':END_ID'>{lower-case($functionName)}</To>
        <Type name=':TYPE'>DECLARES</Type>
    </Record>
};

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

let $r := <LocalFunctions>
{
    (
    for $c in /Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/classes/' || $c/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Entity
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/dataentityviews/' || $c/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Form/Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/../@Package || '/forms/' || $c/../@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Form
    for $control in $c/FormControl
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/forms/' || $c/@Name || '/controls/' || $control/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Form
    for $control in $c/FormDataSource
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/forms/' || $c/@Name || '/datasources/' || $control/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Map
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/maps/' || $c/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Query/Class
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/../@Package || '/queries/' || $c/../@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /View
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/views/' || $c/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName),

    for $c in /Table
    for $m in $c/Method
    for $lm in $m/LocalDeclarationsStatement/FunctionDeclaration
    let $methodName := '/' || $c/@Package || '/tables/' || $c/@Name || '/methods/' || $m/@Name
    let $functionName := $methodName || '/' || $lm/@Name
    return local:Record($methodName, $functionName)
    )
}
</LocalFunctions>

return csv:serialize($r, $options)
