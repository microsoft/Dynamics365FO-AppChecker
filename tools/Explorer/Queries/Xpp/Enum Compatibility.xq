(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Check that assignments and returns types are not incompatible enumerations :)
(: String operations to take Enumeration(NoYes) to NoYes :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:getEnumerationName($s as xs:string) as xs:string
{
    let $r := if ($s = "boolean") then
        "boolean"
    else
        replace($s, "Enumeration\((.*)\)", "$1")
        
    return $r
};

declare function local:isEnumeratedType($s as xs:string) as xs:boolean
{
    let $r := starts-with($s, "Enumeration(")
    return $r
};

declare function local:compatible($t1 as xs:string, $t2 as xs:string) as xs:boolean
{
    let $compatibleTypes := ("NoYes", "FalseTrue", "boolean")
    
    return $t1 = $t2 (: same name :)
      or   ($compatibleTypes = $t1 and $compatibleTypes = $t2)    
};

<Results>
{
    for $c in /*
    for $m in $c//Method
    for $assignment in $m//AssignEqualStatement
    let $lhs := $assignment/*[1]
    let $rhs := $assignment/*[2]
    let $rhstype := string($rhs/@Type)
    let $lhstype := string($lhs/@Type)
    where (local:isEnumeratedType($rhstype) or $rhstype = 'boolean')
      and (local:isEnumeratedType($lhstype) or $lhstype = 'boolean')
      and not (local:compatible(local:getEnumerationName($rhstype), local:getEnumerationName($lhstype))) 
   
    return <Assignment Artifact='{$c/@Artifact}' L='{local:getEnumerationName(string($lhs/@Type))}' R='{local:getEnumerationName(string($rhs/@Type))}'
        StartLine='{$assignment/@StartLine}' EndLine='{$assignment/@EndLine}'
        StartCol='{$assignment/@StartCol}' EndCol='{$assignment/@EndCol}'/>
}
{
    for $c in /*
    for $m in $c//Method
    for $returnStatement in $m//ReturnStatement/*/..
    let $returnType := string($returnStatement/*/@Type)
    let $expectedType := string($returnStatement/@Type) 
    
    where (local:isEnumeratedType($expectedType) or $expectedType = "boolean")
      and (local:isEnumeratedType($returnType) or $returnType = "boolean")
      and not (local:compatible(local:getEnumerationName($expectedType), local:getEnumerationName($returnType)))

    return <Return Artifact='{$c/@Artifact}' L='{local:getEnumerationName($expectedType)}' R='{local:getEnumerationName($returnType)}'
        StartLine='{$returnStatement/@StartLine}' EndLine='{$returnStatement/@EndLine}'
        StartCol='{$returnStatement/@StartCol}' EndCol='{$returnStatement/@EndCol}'/>
    
}
</Results>