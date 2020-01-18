(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find all the places in the transitive class hierarchy where a 
 given method is overridden. The class name and the method name 
 are set in the variables below. :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:TraverseExtensions($name, $extensions)
{
    ($name, for $child in $extensions/Extends[@Parent=$name]  
                return local:TraverseExtensions($child/@Name, $extensions))
};
             
let $extensions := <Extensions>
{
  for $c in /Class
  return <Extends Name='{$c/@Name}' Parent='{$c/@Extends}'/>
}
</Extensions>

let $methodName := "new"
let $className := "SysOperationController"

let $res := local:TraverseExtensions($className, $extensions)

return <Results>
{
    for $name in $res
        let $c := /Class[@Name=$name]
        let $m := $c/Method[@Name=$methodName]
        where $m
        return <Override Artifact='{$c/@Artifact}'  
            StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}'
            EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />
}
</Results>