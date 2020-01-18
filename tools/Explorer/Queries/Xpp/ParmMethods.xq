(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find parm methods. These are the rules (in CheckSignature):
 1) parm methods should not be private (this is debatable)
 2) Setters can be void (but also the same type as argument)
 3) Getters cannot be void
 In checkAssignmentToParameter it is tested that
 4) The parameter is not assigned to. This happens sometimes, and 
    is a clear indication of an error. There will be false positives.
:)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:checkSignature($m as element(Method)) as xs:boolean
{
    let $parameters := count($m/ParameterDeclaration)
    return $m/@IsPrivate="False" and (
        if ($parameters = 1) then
            $m/@Type = ($m/ParameterDeclaration[1]/@Type, "void") 
        else if ($parameters = 0) then
            $m/@Type != "void"
        else false())
};
  
declare function local:checkAssignmentToParameter($m as element(Method)) as xs:boolean
{
    (: If there is a parameter, it should not be assigned to! :)
    if (count($m/ParameterDeclaration) = 1) then
    (
        let $parmName := trace($m/ParameterDeclaration[1]/@Name, "parm")
        
        return not (some $assignment in $m/AssignEqualStatement[not(@GeneratedCodeArtifact)]
        satisfies $assignment/SimpleField[1]/@Name = $parmName)
        
    )
    else
        true()
};

<Results>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  
  where starts-with(lower-case($m/@Name), "parm") 
  and not(local:checkAssignmentToParameter($m)) 
  return <Result Artifact='{$a/@Artifact}'  
    StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}'/>
}
</Results>