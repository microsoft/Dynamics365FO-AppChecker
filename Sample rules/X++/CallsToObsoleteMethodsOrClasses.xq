(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds calls to obsolete methods, and to methods on obsolete classes :)
(: @Language Xpp :)
(: @Category Informational :)

let $calls := 
<Calls>
{
  for $c in /Class
  for $m in $c/Method
  
  for $call in ($m//(StaticMethodCall | QualifiedCall/SimpleQualifier))
    return
         if (fn:local-name($call) ='StaticMethodCall') then
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}' 
                   ClassName='{$call/@ClassName}' MethodName= '{$call/@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
         else (: Instance call :)
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}'  
                   ClassName='{$call/@Type}' MethodName='{$call/../@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
}
</Calls>

return <CallsToObsoleteMethodsOrClasses Category='BestPractice' href='docs.microsoft.com/Socratex/CallsToObsoleteMethodsOrClasses' Version='1.0'>
{
    for $c in $calls/Call
    let $className := $c/@ClassName
    let $target := /Class[@Name=$className]
    where exists($target[@IsObsolete='True']) or exists($target/Method[@Name=$c/@MethodName]/AttributeList/Attribute[@Name='SysObsolete'])
    return $c
}
</CallsToObsoleteMethodsOrClasses>